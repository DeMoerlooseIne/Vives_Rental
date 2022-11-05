using Microsoft.EntityFrameworkCore;
using Vives.Services.Model;
using Vives.Services.Model.Extensions;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services;

public class OrderService : IOrderService
{
    private readonly VivesRentalDbContext _context;

    public OrderService(VivesRentalDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResult?> GetAsync(Guid id)
    {
        var orderDetails = await _context.Orders
            .Where(o => o.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();
        return orderDetails;
    }

    public async Task<List<OrderResult>> FindAsync(OrderFilter? filter = null)
    {
        var orderDetails = await _context.Orders
            .ApplyFilter(filter)
            .MapToResults()
            .ToListAsync();
        return orderDetails;
    }
        
    public async Task<ServiceResult<OrderResult>> CreateAsync(Guid customerId)
    {
        var customer = await _context.Customers
            .Where(c => c.Id == customerId)
            .FirstOrDefaultAsync();

        if (customer == null)
        {
            return new ServiceResult<OrderResult>().DataIsNull("order");
        }

        var order = new Order
        {
            CustomerId = customer.Id,
            CustomerFirstName = customer.FirstName,
            CustomerLastName = customer.LastName,
            CustomerEmail = customer.Email,
            CustomerPhoneNumber = customer.PhoneNumber,
            CreatedAt = DateTime.Now
        };

        var validationResult = ValidationExtensions.IsValid(order);

        if (validationResult.IsSuccess)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var orderResult = new ServiceResult<OrderResult>(await GetAsync(order.Id));
            return orderResult;
        }
        else
        {
            return new ServiceResult<OrderResult>
            {
                Messages = validationResult.Messages
            };
        }
    }

    public async Task<ServiceResult> ReturnAsync(Guid orderId, DateTime returnedAt)
    {
        var orderLines = await _context.OrderLines
            .Where(ol => ol.OrderId == orderId && !ol.ReturnedAt.HasValue)
            .ToListAsync();
        foreach (var orderLine in orderLines)
        {
            orderLine.ReturnedAt = returnedAt;
        }

        var numberOfObjectsUpdated = await _context.SaveChangesAsync();
        if (numberOfObjectsUpdated > 0)
        {
            var serviceResult = new ServiceResult();
            serviceResult.ErrorNoChanges();
        }
        return new ServiceResult();
    }
}