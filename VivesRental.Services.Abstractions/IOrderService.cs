using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface IOrderService
{
    Task<OrderResult?> GetAsync(Guid id);

    Task<List<OrderResult>> FindAsync(OrderFilter? filter);

    Task<OrderResult?> CreateAsync(Guid customerId);
    Task<bool> ReturnAsync(Guid id, DateTime returnedAt);
}