using System.Net.Http.Json;
using Vives.Services.Model;
using VivesRental.Sdk.Extensions;
using VivesRental.Services.Model.Filters;
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

        public async Task<ServiceResult<IList<OrderLineResult>>?> FindAsync(OrderLineFilter? filter)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/orderlines".AddQuery(filter);
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var orderlines = await response.Content.ReadFromJsonAsync <ServiceResult<IList<OrderLineResult>>>();
            if (orderlines is null)
            {
                return new ServiceResult<IList<OrderLineResult>>();
            }
            return orderlines;
        }

        public async Task<ServiceResult<OrderLineResult>?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync <ServiceResult<OrderLineResult>>();
        }

        public async Task<ServiceResult?> RentAsync(Guid orderId,Guid articleId)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{orderId}/rentedorderline";
            var response = await httpClient.PutAsJsonAsync(route, articleId);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult>();
        }
        public async Task<ServiceResult?> RentAsync(Guid orderId, IList<Guid> articleIds)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{orderId}/rentedorderlines";
            var response = await httpClient.PutAsJsonAsync(route, articleIds);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult>();
        }
        public async Task<ServiceResult?> ReturnAsync(Guid Id, DateTime returnedAt)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/orderlines/{Id}/returnedorderline";
            var response = await httpClient.PutAsJsonAsync(route, returnedAt);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult>();
        }
    }
}
