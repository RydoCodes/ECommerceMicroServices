using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<OrderService> logger;

        public OrderService(IHttpClientFactory httpClientFactory,ILogger<OrderService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        public async Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                HttpClient orderserviceclient = httpClientFactory.CreateClient("RydoOrderService");
                HttpResponseMessage response = await orderserviceclient.GetAsync($"api/orders/{customerId}"); // Performs GET for  http://localhost:32205/api/orders/{customerId}
                if (response.IsSuccessStatusCode)
                {
                    byte[] content = await response.Content.ReadAsByteArrayAsync(); // Serialize the HTTP content to a byte array as an asynchronous operation.
                    JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy= JsonNamingPolicy.CamelCase };
                    IEnumerable<Order> result = JsonSerializer.Deserialize<IEnumerable<Order>>(content, options); // Serialize the byte array to <IEnumerable<Order>>
                    return (true, result, null);
                }

                return (false, null, response.ReasonPhrase);
            }
            catch(Exception ex)
            {
                logger?.LogInformation(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
