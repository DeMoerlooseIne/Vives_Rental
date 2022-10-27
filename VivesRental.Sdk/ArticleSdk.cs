using System.Net.Http.Json;
using VivesRental.Model;

namespace VivesRental.Sdk
{
    public class ArticleSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<Article>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/articles";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var articles = await response.Content.ReadFromJsonAsync<IList<Article>>();
            if (articles is null)
            {
                return new List<Article>();
            }

            return articles;
        }

        public async Task<Article?> GetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articles/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Article>();
        }

        public async Task<Article?> CreateAsync(Article article)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/articles";
            var response = await httpClient.PostAsJsonAsync(route, article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Article>();
        }

        public async Task<Article?> UpdateAsync(int id, Article article)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articles/{id}";
            var response = await httpClient.PutAsJsonAsync(route, article);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Article>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/articles/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
