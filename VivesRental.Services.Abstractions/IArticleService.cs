using Vives.Services.Model;
using VivesRental.Enums;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface IArticleService
{
    Task<ArticleResult?> GetAsync(Guid id);
        
    Task<List<ArticleResult>> FindAsync(ArticleFilter? filter = null);
        
    Task<ServiceResult<ArticleResult>> CreateAsync(ArticleRequest entity);
       
    Task<ServiceResult> UpdateStatusAsync(Guid articleId, ArticleStatus status);
    Task<ServiceResult> RemoveAsync(Guid id);
        
}