using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VivesRental.Model;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk
{
    public class OrderSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<OrderResult>> FindAsync(OrderFilter? filter)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orders";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync<IList<OrderResult>>();
            if (orders is null)
            {
                return new List<OrderResult>();
            }

            return orders;
        }

        public async Task<OrderResult?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orders/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderResult>();
        }

        public async Task<OrderResult?> CreateAsync(Guid customerId)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orders";
            var response = await httpClient.PostAsJsonAsync(route, customerId);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderResult>();
        }

        public async Task<bool> ReturnAsync(Guid id, DateTime returnedAt)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orders/{id}";
            var response = await httpClient.PutAsJsonAsync(route, returnedAt);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
