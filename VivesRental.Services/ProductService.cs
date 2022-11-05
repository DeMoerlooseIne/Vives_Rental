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

    public async Task<ProductResult?> GetAsync(Guid id)
    {
        var productDetails = await _context.Products
            .Where(p => p.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();
        return productDetails;
    }

    public async Task<List<ProductResult>> FindAsync(ProductFilter? filter = null)
    { 
        var productDetails = await _context.Products
            .ApplyFilter(filter)
            .MapToResults(filter)
            .ToListAsync();
        return productDetails;
    }

    public async Task<ServiceResult<ProductResult>> CreateAsync(ProductRequest entity)
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
            return new ServiceResult<ProductResult>().DataIsNull("product");
        }

        var validationResult = ValidationExtensions.IsValid(product);

        if (validationResult.IsSuccess)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ServiceResult<ProductResult>(await GetAsync(product.Id));
        }
        else
        {
            return new ServiceResult<ProductResult>
            {
                Messages = validationResult.Messages
            };
        }
    }

    public async Task<ServiceResult<ProductResult>> EditAsync(Guid id, ProductRequest entity)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return new ServiceResult<ProductResult>().DataIsNull("product");
        }

        product.Name = entity.Name;
        product.Description = entity.Description;
        product.Manufacturer = entity.Manufacturer;
        product.Publisher = entity.Publisher;
        product.RentalExpiresAfterDays = entity.RentalExpiresAfterDays;

        var validationResult = ValidationExtensions.IsValid(product);

        if (validationResult.IsSuccess)
        {
            await _context.SaveChangesAsync();
            return new ServiceResult<ProductResult>(await GetAsync(product.Id));
        }
        else
        {
            return new ServiceResult<ProductResult>
            {
                Messages = validationResult.Messages
            };
        }
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
            var serviceResult = new ServiceResult();
            var changes = await _context.SaveChangesAsync();
            if (changes == 0)
            {
                serviceResult.Messages.Add(new ServiceMessage
                {
                    Code = "ProductRemoved",
                    Message = "The product was not deleted.",
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
            var serviceResult = new ServiceResult();
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
    public async Task<ServiceResult> GenerateArticlesAsync(Guid productId, int amount)
    {
        if (amount <= 0 || amount > 10000) //Set a limit to 10K
        {
            var serviceResult = new ServiceResult();
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "InvalAmount",
                Message = "Give up an amount between 0 and 10000.",
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

        var serviceSuccesResult = new ServiceResult();
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