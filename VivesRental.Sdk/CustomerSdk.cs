using System.Net.Http.Json;
using Vives.Services.Model;
using VivesRental.Sdk.Extensions;
using VivesRental.Services.Model.Filters;
using VivesRental.Services.Model.Requests;
using VivesRental.Services.Model.Results;

namespace VivesRental.Sdk
{
    public class CustomerSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ServiceResult<IList<CustomerResult>>?> FindAsync(CustomerFilter? filter)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/customers".AddQuery(filter);
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var customers = await response.Content.ReadFromJsonAsync <ServiceResult<IList<CustomerResult>>>();
            if (customers is null)
            {
                return new ServiceResult<IList<CustomerResult>>();
            }
            return customers;
        }

        public async Task<ServiceResult<CustomerResult>?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/customers/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync <ServiceResult<CustomerResult>>();
        }

        public async Task<ServiceResult<CustomerResult>?> CreateAsync(CustomerRequest customer)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/customers";
            var response = await httpClient.PostAsJsonAsync(route, customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync <ServiceResult<CustomerResult>>();
        }

        public async Task<ServiceResult<CustomerResult>?> EditAsync(Guid id, CustomerRequest customer)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/customers/{id}";
            var response = await httpClient.PutAsJsonAsync(route, customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync <ServiceResult<CustomerResult>>();
        }

        public async Task<ServiceResult?> RemoveAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/customers/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceResult>();
        }
    }
}
