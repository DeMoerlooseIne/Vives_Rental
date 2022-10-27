using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface ICustomerService
{
    Task<CustomerResult?> GetAsync(Guid id);
    Task<List<CustomerResult>> FindAsync(CustomerFilter? filter);
    Task<CustomerResult?> CreateAsync(CustomerRequest entity);
    Task<CustomerResult?> EditAsync(Guid id, CustomerRequest entity);
    Task<bool> RemoveAsync(Guid id);
}