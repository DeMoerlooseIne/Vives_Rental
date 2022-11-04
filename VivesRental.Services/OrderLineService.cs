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

    public async Task<ServiceResult<OrderLineResult?>> GetAsync(Guid id)
    {
        var orderLineDetails = await _context.OrderLines
            .Where(ol => ol.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();

        var serviceResult = new ServiceResult<OrderLineResult?>(orderLineDetails);

        if (serviceResult.Data == null)
        {
            serviceResult.DataIsNull();
        }
        return serviceResult;
    }

    public async Task<ServiceResult<List<OrderLineResult>>> FindAsync(OrderLineFilter? filter = null)
    {
        var orderLineDetails = await _context.OrderLines
            .ApplyFilter(filter)
            .MapToResults()
            .ToListAsync();

        var serviceResult = new ServiceResult<List<OrderLineResult>>();
        
        if (serviceResult.Data == null)
        {
            serviceResult.DataIsNull();
        }
        return serviceResult;
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
            var result = new ServiceResult();
            result.Messages.Add(new ServiceMessage()
            {
                Code = "ArticleNotFound",
                Message = "The article does not exist or is not available.",
                Type = ServiceMessageType.Error
            });
            return result;
        }

        var orderLine = article.CreateOrderLine(orderId);

        _context.OrderLines.Add(orderLine);
        await _context.SaveChangesAsync();

        var successResult = new ServiceResult();
        successResult.Messages.Add(new ServiceMessage()
        {
            Code = "success",
            Message = "The OrderLine wes successfully added.",
            Type = ServiceMessageType.Info
        });
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
            var result = new ServiceResult();
            result.Messages.Add(new ServiceMessage()
            {
                Code = "ArticleNotFound",
                Message = "Article is not available anymore.",
                Type = ServiceMessageType.Error
            });
            return result;
        }

        foreach (var article in articles)
        {
            var orderLine = article.CreateOrderLine(orderId);
            _context.OrderLines.Add(orderLine);
        }

        var numberOfObjectsUpdated = await _context.SaveChangesAsync();
        var serviceSuccesResult = new ServiceResult();
        serviceSuccesResult.Messages.Add(new ServiceMessage()
        {
            Code = "Success",
            Message = $"The {numberOfObjectsUpdated} orderlines were successfully updated.",
            Type = ServiceMessageType.Info
        });
        return serviceSuccesResult;
    }

    /// <summary>
    /// Returns a rented article
    /// </summary>
    /// <param name="orderLineId"></param>
    /// <param name="returnedAt"></param>
    /// <returns></returns>
    public async Task<ServiceResult> ReturnAsync(Guid orderLineId, DateTime returnedAt)
    {
        var orderLine = await _context.OrderLines
            .Where(ol => ol.Id == orderLineId)
            .FirstOrDefaultAsync();

        if (orderLine == null)
        {
            var serviceResult = new ServiceResult<bool>();
            serviceResult.DataIsNull();
            return serviceResult;
        }

        if (returnedAt == DateTime.MinValue)
        {
            var serviceResult = new ServiceResult<bool>();
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "False Date",
                Message = "You need to give an actual date at which the article was returned.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        if (orderLine.ReturnedAt.HasValue)
        {
            var serviceResult = new ServiceResult<bool>();
            serviceResult.Messages.Add(new ServiceMessage()
            {
                Code = "Already returned",
                Message = "This article was already returned.",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        orderLine.ReturnedAt = returnedAt;

        await _context.SaveChangesAsync();

        var serviceSuccessResult = new ServiceResult<bool>(true);
        serviceSuccessResult.Messages.Add(new ServiceMessage()
        {
            Code = "Returned",
            Message = "The article was successfully returned.",
            Type = ServiceMessageType.Info
        });
        return serviceSuccessResult;

    }
}