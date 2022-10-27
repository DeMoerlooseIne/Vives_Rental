using System.Net.Http.Json;
using VivesRental.Model;

namespace VivesRental.Sdk
{
    public class OrderLineSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderLineSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<OrderLine>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orderlines";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var orderlines = await response.Content.ReadFromJsonAsync<IList<OrderLine>>();
            if (orderlines is null)
            {
                return new List<OrderLine>();
            }

            return orderlines;
        }

        public async Task<OrderLine?> GetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderLine>();
        }

        public async Task<OrderLine?> CreateAsync(OrderLine orderline)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orderlines";
            var response = await httpClient.PostAsJsonAsync(route, orderline);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderLine>();
        }

        public async Task<OrderLine?> UpdateAsync(int id, OrderLine orderline)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{id}";
            var response = await httpClient.PutAsJsonAsync(route, orderline);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderLine>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
