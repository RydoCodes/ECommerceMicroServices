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

        // IHttpClientFactory manages HttpClient objects lifecycle
        public CustomerService(IHttpClientFactory httpClientFactory,ILogger<CustomerService> rydologger)
        {
            this.httpClientFactory = httpClientFactory;
            this.rydologger = rydologger;
        }
        public async Task<(bool IsSuccess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                HttpClient customerserviceClient = httpClientFactory.CreateClient("RydoCustomersService");
                HttpResponseMessage response = await customerserviceClient.GetAsync($"/api/Customers/{id}");
                if(response.IsSuccessStatusCode)
                { 
                    byte[] content = await response.Content.ReadAsByteArrayAsync();  // Serialize the HTTP content to a byte array as an asynchronous operation.
                    JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    dynamic result = JsonSerializer.Deserialize<dynamic>(content, options); // Serialize the byte array to <IEnumerable<Order>>
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch(Exception ex) // when one API is off :{"No connection could be made because the target machine actively refused it."}
            {
                return (false, null, ex.ToString());
            }
            
        }
    }
}
