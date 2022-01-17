using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            DbContextOptions<ProductsDbContext> options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                .Options;
            ProductsDbContext dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            ProductProfile productProfile = new ProductProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            Mapper mapper = new Mapper(configuration);

            ProductsProvider productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductsAsync();

            Assert.True(product.IsSuccess);
            Assert.True(product.Products.Any());
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingValidId()
        {
            DbContextOptions<ProductsDbContext> options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnsProductUsingValidId))
                .Options;
            ProductsDbContext dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            ProductProfile productProfile = new ProductProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            Mapper mapper = new Mapper(configuration);

            ProductsProvider productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductsAsync(1);

            Assert.True(product.IsSuccess);
            Assert.NotNull(product.Product);
            Assert.True(product.Product.Id == 1);
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingInvalidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnsProductUsingInvalidId))
                .Options;
            ProductsDbContext dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            ProductProfile productProfile = new ProductProfile();
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            Mapper mapper = new Mapper(configuration);

            ProductsProvider productsProvider = new ProductsProvider(dbContext, null, mapper);

            var product = await productsProvider.GetProductsAsync(-1);
            Assert.False(product.IsSuccess);
            Assert.Null(product.Product);
            Assert.NotNull(product.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.14)
                });
            }
            dbContext.SaveChanges();
        }
    }
}
