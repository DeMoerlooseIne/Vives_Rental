using Vives.Services.Model;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services.Abstractions;

public interface IArticleReservationService
{
    Task<ServiceResult<ArticleReservationResult?>> GetAsync(Guid id);
    Task<ServiceResult<List<ArticleReservationResult>>> FindAsync(ArticleReservationFilter? filter = null);
    Task<ServiceResult<ArticleReservationResult?>> CreateAsync(ArticleReservationRequest entity);
    Task<ServiceResult> RemoveAsync(Guid id);
}