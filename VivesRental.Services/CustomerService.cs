﻿using Microsoft.Data.SqlClient;
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

public class CustomerService : ICustomerService
{
    private readonly VivesRentalDbContext _context;

    public CustomerService(VivesRentalDbContext context)
    {
        _context = context;
    }


    public async Task<ServiceResult<CustomerResult?>> GetAsync(Guid id)
    {
        var customerDetails = await _context.Customers
            .Where(c => c.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();

        var serviceResult = new ServiceResult<CustomerResult?>(customerDetails);

        if (serviceResult.Data == null)
        {
            serviceResult.DataIsNull();
        }            
        return serviceResult;

    }

    public async Task<ServiceResult<List<CustomerResult>>> FindAsync(CustomerFilter? filter = null)
    {
        var customerDetails = await _context.Customers
            .ApplyFilter(filter)
            .MapToResults()
            .ToListAsync();

        var serviceResult = new ServiceResult<List<CustomerResult?>>(customerDetails);

        if (serviceResult.Data == null)
        {
            serviceResult.DataIsNull();
        }
        return serviceResult;
    }

    public async Task<ServiceResult<CustomerResult?>> CreateAsync(CustomerRequest entity)
    {
        var customer = new Customer
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var customerResult = await GetAsync(customer.Id);
        if (customerResult is null)
        {
            var serviceResult = new ServiceResult<CustomerResult>();
            serviceResult.Messages.Add(new ServiceMessage

            {
                Code = "NotFound",
                Message = "Person not found after create",
                Type = ServiceMessageType.Error
            });
            return serviceResult;
        }

        return customerResult;
    }

    public async Task<ServiceResult<CustomerResult?>> EditAsync(Guid id, CustomerRequest entity)
    {
        //Get Product from unitOfWork
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null)
        {
            var serviceResult = new ServiceResult<CustomerResult>();
            serviceResult.DataIsNull();
            return serviceResult;
        }

        //Only update the properties we want to update
        customer.FirstName = entity.FirstName;
        customer.LastName = entity.LastName;
        customer.Email = entity.Email;
        customer.PhoneNumber = entity.PhoneNumber;

        await _context.SaveChangesAsync();

        return await GetAsync(customer.Id);
    }

    /// <summary>
    /// Removes one Customer and disconnects Orders from the customer
    /// </summary>
    /// <param name="id">The id of the Customer</param>
    /// <returns>True if the customer was deleted</returns>
    public async Task<ServiceResult> RemoveAsync(Guid id)
    {
        var serviceResult = new ServiceResult();
        if (_context.Database.IsInMemory())
        {
            await RemoveInternalAsync(id);
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "Customer Deleted",
                Message = "Customer Deleted."
            });
            return serviceResult;
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await RemoveInternalAsync(id);
            await transaction.CommitAsync();
            await transaction.RollbackAsync();
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "Customer Deleted",
                Message = "Customer Deleted."
            });
            return serviceResult;
        }
        catch
        {
            await transaction.RollbackAsync();
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "NothingChanged",
                Message = "Nothing changed."
            });
            return serviceResult;
        }
    }

    private async Task RemoveInternalAsync(Guid id)
    {
        //Remove the Customer from the Orders
        await ClearCustomerAsync(id);
        //Remove the Order
        var customer = new Customer { Id = id };
        _context.Customers.Attach(customer);
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }

    private async Task ClearCustomerAsync(Guid customerId)
    {
        if (_context.Database.IsInMemory())
        {
            var orders = await _context.Orders.Where(ol => ol.CustomerId == customerId).ToListAsync();
            foreach (var order in orders)
            {
                order.CustomerId = null;
            }
            return;
        }

        var commandText = "UPDATE [Order] SET CustomerId = null WHERE CustomerId = @CustomerId";
        var customerIdParameter = new SqlParameter("@CustomerId", customerId);

        await _context.Database.ExecuteSqlRawAsync(commandText, customerIdParameter);
    }
}