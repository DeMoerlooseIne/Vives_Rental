using System.Net.Http.Json;
using Vives.Services.Model;
using VivesRental.Sdk.Extensions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk
{
    public class ArticleReservationSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleReservationSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceResult<IList<ArticleReservationResult>>?> FindAsync(ArticleReservationFilter? filter)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/articlereservations".AddQuery(filter);
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var articlereservations = await response.Content.ReadFromJsonAsync <ServiceResult<IList<ArticleReservationResult>>>();
            if (articlereservations is null)
            {
                return new ServiceResult<IList<ArticleReservationResult>>();
            }

            return articlereservations;
        }

        public async Task<ServiceResult<ArticleReservationResult>?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articlereservations/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync <ServiceResult<ArticleReservationResult>>();
        }

        public async Task<ServiceResult<ArticleReservationResult>?> CreateAsync(ArticleReservationRequest articlereservation)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/articlereservations";
            var response = await httpClient.PostAsJsonAsync(route, articlereservation);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult<ArticleReservationResult>>();
        }

        public async Task<ServiceResult?> RemoveAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articlereservations/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult>();
        }
    }
}
