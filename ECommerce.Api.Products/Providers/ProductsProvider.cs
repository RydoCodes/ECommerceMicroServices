using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;
        public ProductsProvider(ProductsDbContext dbContext,ILogger<ProductsProvider> logger, IMapper mapper) 
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;

            SeedData();
        }

        private void SeedData()
        {
            if(!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Db.Product() { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
                dbContext.Products.Add(new Db.Product() { Id = 2, Name = "Monitor", Price = 5, Inventory = 100 });
                dbContext.Products.Add(new Db.Product() { Id = 3, Name = "Mouse", Price = 150, Inventory = 100 });
                dbContext.Products.Add(new Db.Product() { Id = 4, Name = "CPU", Price = 200, Inventory = 100 });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                logger?.LogInformation("Getting All Customers");
                var products = await dbContext.Products.ToListAsync();
                if(products!=null && products.Any())
                {
                   var result = mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);
                    return (true, result, null);
                }
                return (false, null, "Products Not Found");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Product Product, string ErrorMessage)> GetProductsAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p=>p.Id==id);
                if (product != null)
                {
                    var result = mapper.Map<Db.Product, Models.Product>(product);
                    return (true, result, null);
                }
                return (false, null, "Product Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
