using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Vives.Services.Model;
using VivesRental.Model;
using VivesRental.Sdk.Extensions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk
{
    public class ProductSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<ProductResult>> FindAsync(ProductFilter? filter)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/products".AddQuery(filter);
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadFromJsonAsync <IList<ProductResult>>();
            if (products is null)
            {
                return new List<ProductResult>();
            }
            return products;
        }

        public async Task<ProductResult?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/products/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductResult>();
        }

        public async Task<ServiceResult<ProductResult>?> CreateAsync(ProductRequest product)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/products";
            var response = await httpClient.PostAsJsonAsync(route, product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync <ServiceResult<ProductResult>>();
        }

        public async Task<ServiceResult<ProductResult>?> EditAsync(Guid id, ProductRequest product)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/products/{id}";
            var response = await httpClient.PutAsJsonAsync(route, product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync <ServiceResult<ProductResult>>();
        }

        public async Task<ServiceResult?> RemoveAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/products/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult>();
        }

        public async Task<ServiceResult?> GenerateArticlesAsync(Guid Id, int amount)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/products/{Id}";
            var response = await httpClient.PutAsJsonAsync(route, amount);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult>();
        }
    }
}
