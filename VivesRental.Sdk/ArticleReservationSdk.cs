using System.Net.Http.Json;
using VivesRental.Model;

namespace VivesRental.Sdk
{
    public class ArticleReservationSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleReservationSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<ArticleReservation>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/articlereservations";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var articlereservations = await response.Content.ReadFromJsonAsync<IList<ArticleReservation>>();
            if (articlereservations is null)
            {
                return new List<ArticleReservation>();
            }

            return articlereservations;
        }

        public async Task<ArticleReservation?> GetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articlereservations/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ArticleReservation>();
        }

        public async Task<ArticleReservation?> CreateAsync(ArticleReservation articlereservation)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/articlereservations";
            var response = await httpClient.PostAsJsonAsync(route, articlereservation);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ArticleReservation>();
        }

        public async Task<ArticleReservation?> UpdateAsync(int id, ArticleReservation articlereservation)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articlereservations/{id}";
            var response = await httpClient.PutAsJsonAsync(route, articlereservation);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ArticleReservation>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articlereservations/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
