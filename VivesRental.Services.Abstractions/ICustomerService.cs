using Vives.Services.Model;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface ICustomerService
{
    Task<ServiceResult<CustomerResult?>> GetAsync(Guid id);
    Task<ServiceResult<List<CustomerResult>>> FindAsync(CustomerFilter? filter);
    Task<ServiceResult<CustomerResult?>> CreateAsync(CustomerRequest entity);
    Task<ServiceResult<CustomerResult?>> EditAsync(Guid id, CustomerRequest entity);
    Task<ServiceResult> RemoveAsync(Guid id);
}