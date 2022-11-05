using Vives.Services.Model;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface IOrderLineService
{
    Task<OrderLineResult?> GetAsync(Guid id);
    Task<ServiceResult> RentAsync(Guid orderId, Guid articleId);
    Task<ServiceResult> RentAsync(Guid orderId, IList<Guid> articleIds);
    Task<ServiceResult> ReturnAsync(Guid orderLineId, DateTime returnedAt);
    Task<List<OrderLineResult>> FindAsync(OrderLineFilter? filter);

}