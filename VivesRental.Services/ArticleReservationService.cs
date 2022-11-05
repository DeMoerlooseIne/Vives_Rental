using Microsoft.EntityFrameworkCore;
using Vives.Services.Model;
using Vives.Services.Model.Extensions;
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
    public async Task<ArticleReservationResult?> GetAsync(Guid id)
    {
        var articleReservationDetails = await _context.ArticleReservations
            .Where(ar => ar.Id == id)
            .MapToResults()
            .FirstOrDefaultAsync();
        return articleReservationDetails;
    }
        
    public async Task<List<ArticleReservationResult>> FindAsync(ArticleReservationFilter? filter = null)
    {
        var articleReservationDetails = await _context.ArticleReservations
            .ApplyFilter(filter)
            .MapToResults()
            .ToListAsync();
        return articleReservationDetails;
    }
        
    public async Task<ServiceResult<ArticleReservationResult>> CreateAsync(ArticleReservationRequest request)
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

        if (articleReservation == null)
        {
            return new ServiceResult<ArticleReservationResult>().DataIsNull("articlereservation");
        }

        var validationResult = ValidationExtensions.IsValid(articleReservation);

        if (validationResult.IsSuccess)
        {
            _context.ArticleReservations.Add(articleReservation);
            await _context.SaveChangesAsync();
            return new ServiceResult<ArticleReservationResult>(await GetAsync(articleReservation.Id));
        }
        else
        {
            return new ServiceResult<ArticleReservationResult>
            {
                Messages = validationResult.Messages
            };
        }
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
        
        var serviceResult = new ServiceResult();
        var changes = await _context.SaveChangesAsync();
        if (changes is 0)
        {
            serviceResult.Messages.Add(new ServiceMessage
            {
                Code = "NothingChanged",
                Message = "Something happend... nothing changed in the database.",
                Type = ServiceMessageType.Error
            });
        }
        return serviceResult;
    }
}