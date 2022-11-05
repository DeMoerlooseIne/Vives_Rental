using System.Net.Http.Json;
using Vives.Services.Model;
using VivesRental.Sdk.Extensions;
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
            var route = "/api/articles".AddQuery(filter);
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var articles = await response.Content.ReadFromJsonAsync<ServiceResult<IList<ArticleResult>>>();
            if (articles is null)
            {
                return new ServiceResult<IList<ArticleResult>>();
            }

            return articles;
        }

        public async Task<ServiceResult<ArticleResult>?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articles/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult<ArticleResult>>();
        }

        public async Task<ServiceResult<ArticleResult>?> CreateAsync(ArticleRequest article)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/articles";
            var response = await httpClient.PostAsJsonAsync(route, article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult<ArticleResult>>();
        }

        public async Task<ServiceResult<ArticleResult>?> UpdateStatusAsync(Guid articleId, ArticleRequest article)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articles/{articleId}";
            var response = await httpClient.PutAsJsonAsync(route, article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult<ArticleResult>>();
        }

        public async Task<ServiceResult?> RemoveAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articles/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult>();
        }
    }
}
