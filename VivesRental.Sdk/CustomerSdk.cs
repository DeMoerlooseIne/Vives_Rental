using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VivesRental.Model;

namespace VivesRental.Sdk
{
    public class CustomerSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<Customer>> FindAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/customers";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            var customers = await response.Content.ReadFromJsonAsync<IList<Customer>>();
            if (customers is null)
            {
                return new List<Customer>();
            }

            return customers;
        }

        public async Task<Customer?> GetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/customers/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Customer>();
        }

        public async Task<Customer?> CreateAsync(Customer customer)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = "/api/customers";
            var response = await httpClient.PostAsJsonAsync(route, customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Customer>();
        }

        public async Task<Customer?> UpdateAsync(int id, Customer customer)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/customers/{id}";
            var response = await httpClient.PutAsJsonAsync(route, customer);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Customer>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("VivesRentalApi");
            var route = $"/api/customers/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }
    }
}
