using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.Sdk
{
    public class OrderSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<Order>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orders";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync<IList<Order>>();
            if (orders is null)
            {
                return new List<Order>();
            }

            return orders;
        }

        public async Task<Order?> GetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orders/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
        }

        public async Task<Order?> CreateAsync(Order order)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orders";
            var response = await httpClient.PostAsJsonAsync(route, order);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
        }

        public async Task<Order?> UpdateAsync(int id, Order order)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orders/{id}";
            var response = await httpClient.PutAsJsonAsync(route, order);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Order>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orders/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
