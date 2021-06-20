using ECommerce.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<CustomerService> rydologger;

        public CustomerService(IHttpClientFactory httpClientFactory,ILogger<CustomerService> rydologger)
        {
            this.httpClientFactory = httpClientFactory;
            this.rydologger = rydologger;
        }
        public async Task<(bool IsSuccess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customerClient = httpClientFactory.CreateClient("RydoCustomersService");
                var response = await customerClient.GetAsync($"/api/Customers/{id}");
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<dynamic>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch(Exception ex)
            {
                return (false, null, ex.ToString());
            }
            
        }
    }
}
