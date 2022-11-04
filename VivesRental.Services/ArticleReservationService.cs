using Microsoft.EntityFrameworkCore;
using Vives.Services.Model;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Abstractions;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Services;

public class ArticleReservationService : IArticleReservationService
{
    private readonly VivesRentalDbContext _context;

    public ArticleReservationService(VivesRentalDbContext context)
    {
        _context = context;
    }
    public async Task<ServiceResult<ArticleReservationResult?>> GetAsync(Guid id)
    {
        var serviceResult = new ServiceResult<ArticleReservationResult?>();

        var articleReservationDetails = await _context.ArticleReservations
            .Where(ar => ar.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();

        if (serviceResult.IsSuccess)
            serviceResult.Messages.AsParallel();
        else
            serviceResult.Data.Equals(articleReservationDetails);

        return serviceResult;
    }
        
    public async Task<ServiceResult<List<ArticleReservationResult>>> FindAsync(ArticleReservationFilter? filter = null)
    {
        var serviceResult = new ServiceResult<List<ArticleReservationResult?>>();

        var articleReservationDetails = await _context.ArticleReservations
            .ApplyFilter(filter)
            .MapToResults()
            .ToListAsync();

        if (serviceResult.IsSuccess)
            serviceResult.Messages.AsParallel();
        else
            serviceResult.Data.Equals(articleReservationDetails);

        return serviceResult;
    }
        
    public async Task<ServiceResult<ArticleReservationResult?>> CreateAsync(ArticleReservationRequest request)
    {
        request.FromDateTime ??= DateTime.Now;
        request.UntilDateTime ??= DateTime.Now.AddMinutes(5);

        var articleReservation = new ArticleReservation
        {
            CustomerId = request.CustomerId,
            ArticleId = request.ArticleId,
            FromDateTime = request.FromDateTime.Value,
            UntilDateTime = request.UntilDateTime.Value
        };

        _context.ArticleReservations.Add(articleReservation);

        await _context.SaveChangesAsync();

        return await GetAsync(articleReservation.Id);
    }

    /// <summary>
    /// Removes one ArticleReservation
    /// </summary>
    /// <param name="id">The id of the ArticleReservation</param>
    /// <returns>True if the article reservation was deleted</returns>
    public async Task<ServiceResult> RemoveAsync(Guid id)
    {
        var articleReservation = new ArticleReservation { Id = id };
        _context.ArticleReservations.Attach(articleReservation);
        _context.ArticleReservations.Remove(articleReservation);

        await _context.SaveChangesAsync();

        var serviceResult = new ServiceResult();
        serviceResult.Messages.Add(new ServiceMessage()
        {
            Code = "ArticleReservationDeleted",
            Message = "ArticleReservation was successfully deleted.",
            Type = ServiceMessageType.Info
        });
        return serviceResult;
    }
}