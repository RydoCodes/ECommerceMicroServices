using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomerService customerService;

        public SearchService(IOrderService ordersService,IProductsService productsService,ICustomerService customerService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            //await Task.Delay(1);
            //return (true, new { Message = "Hello" });

            var orderResult = await ordersService.GetOrdersAsync(customerId);

            var productResult = await productsService.GetProductsAsync();

            var customerresult = await customerService.GetCustomerAsync(customerId);

            if (orderResult.IsSuccess)
            {

                foreach(var order in orderResult.Orders)
                {
                    foreach(var item in order.Items)
                    {
                        item.ProductName = productResult.IsSuccess ? productResult.products.FirstOrDefault(p => p.Id == item.ProductId)?.Name : "Product Name is not available";
                    }
                }

                var result = new
                {
                    Customer = customerresult.IsSuccess? customerresult.Customer : new { Name ="Customer Information Not available"},
                    Orders = orderResult.Orders
                };
                return (true, result);
            }
            return (false, null);



        }
    }
}
