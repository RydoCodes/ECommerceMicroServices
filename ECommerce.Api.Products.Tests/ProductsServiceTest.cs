using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async void GetProductsReturnsAllProducts()
        {
            var rydooptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(rydooptions);

            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));

            var mapper = new Mapper(configuration);

            var productsprovider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsprovider.GetProductsAsync();

            

            Assert.True(product.IsSuccess);
            Assert.True(product.Products.Any());
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async void GetProductReturnsProductUsingValidId()
        {
            var rydooptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(rydooptions);

            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));

            var mapper = new Mapper(configuration);

            var productsprovider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsprovider.GetProductsAsync(1);

            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Product);
            Assert.True(product.Product.Id == 1);
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async void GetProductReturnsProductUsingInvalidId()
        {
            var rydooptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(rydooptions);

            CreateProducts(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));

            var mapper = new Mapper(configuration);

            var productsprovider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsprovider.GetProductsAsync(-1);

            Assert.False(product.IsSuccess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            for(int i=1;i<=10;i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)3.14 * i
                }); ; 
            }
            dbContext.SaveChanges();
        }
    }
}   
