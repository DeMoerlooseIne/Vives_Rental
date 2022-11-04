using System.Net.Http.Json;
using Vives.Services.Model;
using VivesRental.Sdk.Extensions;
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

        public async Task<ServiceResult<IList<OrderResult>>?> FindAsync(OrderFilter? filter)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orders".AddQuery(filter);
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadFromJsonAsync <ServiceResult<IList<OrderResult>>>();
            if (orders is null)
            {
                return new ServiceResult<IList<OrderResult>>();
            }
            return orders;
        }

        public async Task<ServiceResult<OrderResult>?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orders/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync <ServiceResult<OrderResult>>();
        }

        public async Task<ServiceResult<OrderResult>?> CreateAsync(Guid customerId)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orders";
            var response = await httpClient.PostAsJsonAsync(route, customerId);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync <ServiceResult<OrderResult>>();
        }

        public async Task<ServiceResult?> ReturnAsync(Guid id, DateTime returnedAt)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orders/{id}";
            var response = await httpClient.PutAsJsonAsync(route, returnedAt);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult>();
        }
    }
}
