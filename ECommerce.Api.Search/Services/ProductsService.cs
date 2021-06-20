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
                var productserviceclient = rydoclient.CreateClient("RydoProductsService");
                var response = await productserviceclient?.GetAsync("api/products");
                if(response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<IEnumerable<Product>>(data, options);
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
