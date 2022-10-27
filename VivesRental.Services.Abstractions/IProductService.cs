using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface IProductService
{
    Task<ProductResult?> GetAsync(Guid id);
    Task<List<ProductResult>> FindAsync(ProductFilter? filter);
    Task<ProductResult?> CreateAsync(ProductRequest entity);
    Task<ProductResult?> EditAsync(Guid id, ProductRequest entity);
    Task<bool> RemoveAsync(Guid id);
    Task<bool> GenerateArticlesAsync(Guid productId, int amount);

}