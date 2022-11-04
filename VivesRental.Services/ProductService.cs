using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Vives.Services.Model;
using Vives.Services.Model.Extensions;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services;

public class ProductService : IProductService
{
    private readonly VivesRentalDbContext _context;
    public ProductService(VivesRentalDbContext context)
    {
        _context = context;
    }


    public async Task<ServiceResult<ProductResult?>> GetAsync(Guid id)
    {
        var productDetails = await _context.Products
            .Where(p => p.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();

        var serviceResult = new ServiceResult<ProductResult?>(productDetails);

        if (serviceResult.Data == null)
        {
            serviceResult.DataIsNull();
        }
        return serviceResult;
    }

    public async Task<ServiceResult<List<ProductResult>>> FindAsync(ProductFilter? filter = null)
    { 
        var productDetails = await _context.Products
            .ApplyFilter(filter)
            .MapToResults(filter)
            .ToListAsync();

        var serviceResult = new ServiceResult<List<ProductResult?>>();

        if (serviceResult.Data == null)
        {
            serviceResult.DataIsNull();
        }
        return serviceResult;
    }

    public async Task<ServiceResult<ProductResult?>> CreateAsync(ProductRequest entity)
    {
        var product = new Product
        {
            Name = entity.Name,
            Description = entity.Description,
            Manufacturer = entity.Manufacturer,
            Publisher = entity.Publisher,
            RentalExpiresAfterDays = entity.RentalExpiresAfterDays
        };

        if (product == null)
        {
            var serviceResult = new ServiceResult<ProductResult?>();
            serviceResult.DataIsNull();
            return serviceResult;
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return await GetAsync(product.Id);
    }

    public async Task<ServiceResult<ProductResult?>> EditAsync(Guid id, ProductRequest entity)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return null;
        }
            
        product.Name = entity.Name;
        product.Description = entity.Description;
        product.Manufacturer = entity.Manufacturer;
        product.Publisher = entity.Publisher;
        product.RentalExpiresAfterDays = entity.RentalExpiresAfterDays;

        await _context.SaveChangesAsync();

        return await GetAsync(product.Id);
    }

    /// <summary>
    /// Removes one Product, removes ArticleReservations, removes all linked articles and disconnects OrderLines from articles
    /// </summary>
    /// <param name="id">The id of the Product</param>
    /// <returns>True if the product was deleted</returns>
    public async Task<ServiceResult> RemoveAsync(Guid id)
    {
        var product = new Product { Id = id };

        if (_context.Database.IsInMemory())
        {
            _context.Products.Attach(product);
            _context.Products.Remove(product);
            var serviceResult = new ServiceResult<bool>();
            var changes = await _context.SaveChangesAsync();
            if (changes == 0)
            {
                serviceResult.Messages.Add(new ServiceMessage
                {
                    Code = "ProductRemoved",
                    Message = "The product was succesfully deleted.",
                    Type = ServiceMessageType.Error
                });
            }
            return serviceResult;
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await RemoveInternalAsync(id);
            await transaction.CommitAsync();
            var serviceResult = new ServiceResult<bool>(true);
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "ProductRemoved",
                Message = "The product was successfully deleted.",
                Type = ServiceMessageType.Info
            });
            return serviceResult;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task RemoveInternalAsync(Guid id)
    {
        await ClearArticleByProductIdAsync(id);
        _context.ArticleReservations.RemoveRange(
            _context.ArticleReservations.Where(a => a.Article.ProductId == id));
        _context.Articles.RemoveRange(_context.Articles.Where(a => a.ProductId == id));

        //Remove product
        var product = new Product { Id = id };
        _context.Products.Attach(product);
        _context.Products.Remove(product);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Adds a number of articles with Normal status to the Product.
    /// This is limited to maximum 10.000
    /// </summary>
    /// <returns>True if articles are added</returns>
    public async Task<ServiceResult<bool>> GenerateArticlesAsync(Guid productId, int amount)
    {
        if (amount <= 0 || amount > 10000) //Set a limit to 10K
        {
            var serviceResult = new ServiceResult<bool>();
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "IncorrectAmount",
                Message = "Give up a different amount.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        for (int i = 0; i < amount; i++)
        {
            var article = new Article
            {
                ProductId = productId
            };
            _context.Articles.Add(article);
        }

        var numberOfObjectsUpdated = await _context.SaveChangesAsync();

        var serviceSuccesResult = new ServiceResult<bool>(numberOfObjectsUpdated > 0);
        serviceSuccesResult.Messages.Add(new ServiceMessage()
        {
            Code = "Success",
            Message = $"The {numberOfObjectsUpdated} articles were successfully added.",
            Type = ServiceMessageType.Info
        });
        return serviceSuccesResult;
    }
        
    private async Task ClearArticleByProductIdAsync(Guid productId)
    {
        if (_context.Database.IsInMemory())
        {
            var orderLines = await _context.OrderLines
                .Where(ol => ol.Article.ProductId == productId)
                .ToListAsync();
            foreach (var orderLine in orderLines)
            {
                orderLine.ArticleId = null;
            }

            return;
        }

        var commandText =
            "UPDATE OrderLine SET ArticleId = null from OrderLine inner join Article on Article.ProductId = @ProductId";
        var articleIdParameter = new SqlParameter("@ProductId", productId);

        await _context.Database.ExecuteSqlRawAsync(commandText, articleIdParameter);
    }
}