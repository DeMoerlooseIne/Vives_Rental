using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface IOrderLineService
{
    Task<OrderLineResult?> GetAsync(Guid id);
    Task<bool> RentAsync(Guid orderId, Guid articleId);
    Task<bool> RentAsync(Guid orderId, IList<Guid> articleIds);
    Task<bool> ReturnAsync(Guid orderLineId, DateTime returnedAt);
    Task<List<OrderLineResult>> FindAsync(OrderLineFilter? filter);

}