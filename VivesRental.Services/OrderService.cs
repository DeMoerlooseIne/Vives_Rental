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

    public async Task<ServiceResult<OrderResult?>> GetAsync(Guid id)
    {
        var orderDetails = await _context.Orders
            .Where(o => o.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();

        var serviceResult = new ServiceResult<OrderResult?>(orderDetails);
        if (serviceResult.Data == null)
        {
            serviceResult.DataIsNull();
        }
        return serviceResult;
    }

    public async Task<ServiceResult<List<OrderResult>>> FindAsync(OrderFilter? filter = null)
    {
        var orderDetails = await _context.Orders
            .ApplyFilter(filter)
            .MapToResults()
            .ToListAsync();

        var serviceResult = new ServiceResult<List<OrderResult>>(orderDetails);
        if (serviceResult.Data == null)
        {
            serviceResult.DataIsNull();
        }
        return serviceResult;
    }
        
    public async Task<ServiceResult<OrderResult?>> CreateAsync(Guid customerId)
    {
        var customer = await _context.Customers
            .Where(c => c.Id == customerId)
            .FirstOrDefaultAsync();

        if (customer == null)
        {
            var serviceResult = new ServiceResult<OrderResult?>();
            serviceResult.DataIsNull();
            return serviceResult;
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

        _context.Orders.Add(order);
        var numberOfObjectsUpdated = await _context.SaveChangesAsync();
        if (numberOfObjectsUpdated > 0)
        {
            var serviceResult = await GetAsync(order.Id);
            return serviceResult;
        }

        var failedResult = new ServiceResult<OrderResult?>(null);
        failedResult.ErrorNoChanges();
        return failedResult;
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