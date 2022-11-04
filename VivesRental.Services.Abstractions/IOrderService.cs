using Vives.Services.Model;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface IOrderService
{
    Task<ServiceResult<OrderResult?>> GetAsync(Guid id);
    Task<ServiceResult<List<OrderResult>>> FindAsync(OrderFilter? filter);
    Task<ServiceResult<OrderResult?>> CreateAsync(Guid customerId);
    Task<ServiceResult> ReturnAsync(Guid id, DateTime returnedAt);
}