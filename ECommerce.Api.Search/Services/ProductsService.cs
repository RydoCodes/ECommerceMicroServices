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
    public class ProductsService : IProductsService
    {
        private readonly IHttpClientFactory rydoclient;
        private readonly ILogger rydologger;

        public ProductsService(IHttpClientFactory rydoclient, ILogger<ProductsService> rydologger)
        {
            this.rydoclient = rydoclient;
            this.rydologger = rydologger;
        }
        public async Task<(bool IsSuccess, IEnumerable<Product> products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                HttpClient productserviceclient = rydoclient.CreateClient("RydoProductsService");
                HttpResponseMessage response = await productserviceclient?.GetAsync("api/products");  // Performs GET for  http://localhost:32198/api/products
                if (response.IsSuccessStatusCode)
                {
                    byte[] data = await response.Content.ReadAsByteArrayAsync(); // Serialize the HTTP content to a byte array as an asynchronous operation.
                    JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    IEnumerable<Product> result = JsonSerializer.Deserialize<IEnumerable<Product>>(data, options); // Serialize the byte array to <IEnumerable<Order>>
                    return (true, result, null);
                }

                return (false, null, response.ReasonPhrase);

            }
            catch(Exception ex)
            {
                rydologger?.LogInformation(ex.ToString());
                return (false, null, ex.ToString());
            }
        }
    }
}
