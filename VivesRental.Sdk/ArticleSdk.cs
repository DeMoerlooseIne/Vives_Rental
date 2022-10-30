using System.Net.Http.Json;
using VivesRental.Model;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk
{
    public class ArticleSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<ArticleResult>> FindAsync(ArticleFilter? filter)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/articles";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var articles = await response.Content.ReadFromJsonAsync<IList<ArticleResult>>();
            if (articles is null)
            {
                return new List<ArticleResult>();
            }

            return articles;
        }

        public async Task<ArticleResult?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articles/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ArticleResult>();
        }

        public async Task<ArticleResult?> CreateAsync(ArticleRequest article)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/articles";
            var response = await httpClient.PostAsJsonAsync(route, article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ArticleResult>();
        }

        public async Task<ArticleResult?> UpdateStatusAsync(Guid articleId, ArticleResult article)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articles/{articleId}";
            var response = await httpClient.PutAsJsonAsync(route, article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ArticleResult>();
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articles/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
