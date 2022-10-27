using VivesRental.Enums;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface IArticleService
{
    Task<ArticleResult?> GetAsync(Guid id);
        
    Task<List<ArticleResult>> FindAsync(ArticleFilter? filter = null);
        
    Task<ArticleResult?> CreateAsync(ArticleRequest entity);
       
    Task<bool> UpdateStatusAsync(Guid articleId, ArticleStatus status);
    Task<bool> RemoveAsync(Guid id);
        
}