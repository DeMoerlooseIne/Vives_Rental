using Microsoft.EntityFrameworkCore;
using Vives.Services.Model;
using Vives.Services.Model.Extensions;
using VivesRental.Repository.Core;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services;

public class OrderLineService : IOrderLineService
{
    private readonly VivesRentalDbContext _context;

    public OrderLineService(VivesRentalDbContext context)
    {
        _context = context;
    }

    public async Task<OrderLineResult?> GetAsync(Guid id)
    {
        var orderLineDetails = await _context.OrderLines
            .Where(ol => ol.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();
        return orderLineDetails;
    }

    public async Task<List<OrderLineResult>> FindAsync(OrderLineFilter? filter = null)
    {
        var orderLineDetails = await _context.OrderLines
            .ApplyFilter(filter)
            .MapToResults()
            .ToListAsync();
        return orderLineDetails;
    }

    public async Task<ServiceResult> RentAsync(Guid orderId, Guid articleId)
    {
        var fromDateTime = DateTime.Now;
        var articleFilter = new ArticleFilter
        {
            AvailableFromDateTime = fromDateTime
        };
        var article = await _context.Articles
            .Include(a => a.Product)
            .Where(a => a.Id == articleId)
            .ApplyFilter(articleFilter)
            .SingleOrDefaultAsync();

        if (article == null)
        {
            //Article does not exist or is not available.
            var serviceResult = new ServiceResult();
            serviceResult.NotFound("article");
            return serviceResult;
        }

        var orderLine = article.CreateOrderLine(orderId);

        _context.OrderLines.Add(orderLine);
        await _context.SaveChangesAsync();

        var successResult = new ServiceResult();
        successResult.SuccesfullyAdded("orderline");
        return successResult;
    }

    public async Task<ServiceResult> RentAsync(Guid orderId, IList<Guid> articleIds)
    {
        var articleFilter = new ArticleFilter
        {
            ArticleIds = articleIds,
            AvailableFromDateTime = DateTime.Now
        };
            
        var articles = await _context.Articles
            .Include(a => a.Product) //Needs include for the CreateOrderLine extension method.
            .ApplyFilter(articleFilter)
            .ToListAsync();

        //If the amount of articles is not the same as the requested ids, some articles are not available anymore
        if (articleIds.Count != articles.Count)
        {
            var serviceResult = new ServiceResult();
            serviceResult.NotFound("article");
            return serviceResult;
        }

        foreach (var article in articles)
        {
            var orderLine = article.CreateOrderLine(orderId);
            _context.OrderLines.Add(orderLine);
        }

        var numberOfObjectsUpdated = await _context.SaveChangesAsync();
        if (numberOfObjectsUpdated is 0)
        {
            var serviceResult = new ServiceResult();
            serviceResult.NoChanges();
            return serviceResult;
        }
        return new ServiceResult();
    }

    /// Returns a rented article
    public async Task<ServiceResult> ReturnAsync(Guid orderLineId, DateTime returnedAt)
    {
        var orderLine = await _context.OrderLines
            .Where(ol => ol.Id == orderLineId)
            .FirstOrDefaultAsync();

        if (orderLine == null)
        {
            var serviceResult = new ServiceResult();
            serviceResult.DataIsNull("orderline");
            return serviceResult;
        }

        if (returnedAt == DateTime.MinValue)
        {
            var serviceResult = new ServiceResult();
            serviceResult.InvalidDate();
            return serviceResult;
        }

        if (orderLine.ReturnedAt.HasValue)
        {
            var serviceResult = new ServiceResult();
            serviceResult.ArticleAlreadyReturned();
            return serviceResult;
        }

        orderLine.ReturnedAt = returnedAt;

        await _context.SaveChangesAsync();

        var serviceSuccessResult = new ServiceResult();
        serviceSuccessResult.SuccesfullyReturned("article");
        return serviceSuccessResult;

    }
}