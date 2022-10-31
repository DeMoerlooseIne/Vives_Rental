using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VivesRental.Model;
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

        public async Task<IList<CustomerResult>> FindAsync(CustomerFilter? filter)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/customers";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var customers = await response.Content.ReadFromJsonAsync<IList<CustomerResult>>();
            if (customers is null)
            {
                return new List<CustomerResult>();
            }
            return customers;
        }

        public async Task<CustomerResult?> GetAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/customers/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerResult>();
        }

        public async Task<CustomerResult?> CreateAsync(CustomerRequest customer)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/customers";
            var response = await httpClient.PostAsJsonAsync(route, customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerResult>();
        }

        public async Task<CustomerResult?> EditAsync(Guid id, CustomerRequest customer)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/customers/{id}";
            var response = await httpClient.PutAsJsonAsync(route, customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CustomerResult>();
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/customers/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
