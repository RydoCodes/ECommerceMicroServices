using ECommerce.Api.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        //returning a tuple
        Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync();
        Task<(bool IsSuccess, Product Product, string ErrorMessage)> GetProductsAsync(int id);
    }
}
