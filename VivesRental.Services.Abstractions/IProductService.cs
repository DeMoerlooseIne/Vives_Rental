using Vives.Services.Model;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface IProductService
{
    Task<ServiceResult<ProductResult?>> GetAsync(Guid id);
    Task<ServiceResult<List<ProductResult>>> FindAsync(ProductFilter? filter);
    Task<ServiceResult<ProductResult?>> CreateAsync(ProductRequest entity);
    Task<ServiceResult<ProductResult?>> EditAsync(Guid id, ProductRequest entity);
    Task<ServiceResult> RemoveAsync(Guid id);
    Task<ServiceResult> GenerateArticlesAsync(Guid productId, int amount);

}