using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.Sdk
{
    public class ProductSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<Product>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/products";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadFromJsonAsync<IList<Product>>();
            if (products is null)
            {
                return new List<Product>();
            }

            return products;
        }

        public async Task<Product?> GetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/products/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task<Product?> CreateAsync(Product product)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/products";
            var response = await httpClient.PostAsJsonAsync(route, product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/products/{id}";
            var response = await httpClient.PutAsJsonAsync(route, product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/products/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
