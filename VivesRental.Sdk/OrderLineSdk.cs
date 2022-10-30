using System.Net.Http.Json;
using VivesRental.Model;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk
{
    public class OrderLineSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderLineSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<OrderLineResult>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orderlines";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var orderlines = await response.Content.ReadFromJsonAsync<IList<OrderLineResult>>();
            if (orderlines is null)
            {
                return new List<OrderLineResult>();
            }

            return orderlines;
        }

        public async Task<OrderLineResult?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderLineResult>();
        }

        public async Task<bool> RentAsync(Guid orderId,Guid articleId)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{orderId}";
            var response = await httpClient.PutAsJsonAsync(route, articleId);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
        public async Task<bool> RentAsync(Guid orderId, IList<Guid> articleIds)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{orderId}";
            var response = await httpClient.PutAsJsonAsync(route, articleIds);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
        public async Task<bool> ReturnAsync(Guid orderLineId, DateTime returnedAt)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{orderLineId}";
            var response = await httpClient.PutAsJsonAsync(route, returnedAt);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
