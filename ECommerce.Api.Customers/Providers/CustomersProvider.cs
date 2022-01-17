using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;

        public readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Customer() { Id = 1, Name = "Vaibhav", Address = "Flat no 101, Faridabad" });
                dbContext.Customers.Add(new Customer() { Id = 2, Name = "Nitin", Address = "Gurgaon" });
                dbContext.Customers.Add(new Customer() { Id = 3, Name = "Ishu", Address = "L Blck Kanpur" });
                dbContext.SaveChanges();
            }
        }
        public async Task<(bool IsSuccess, IEnumerable<Customerdto> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                logger?.LogInformation("Getting Customers by ID");

                List<Customer> Employees = await dbContext.Customers.ToListAsync();
                if (Employees != null && Employees.Any())
                {
                    IEnumerable<Customerdto> result = mapper.Map<IEnumerable<Customer>, IEnumerable<Models.Customerdto>>(Employees);
                    return (true, result, null);
                }
                return (false, null, "Customers not available");
            }
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Customerdto Customer, string ErrorMessage)> GetCustomersAsync(int id)
        {
            try
            {
                logger?.LogInformation("Getting a Customer by ID");

                Db.Customer Customer = await dbContext.Customers.FirstAsync(c => c.Id == id);
                if (Customer != null)
                {
                    var result = mapper.Map<Customer, Customerdto>(Customer);
                    return (true, result, null);
                }
                return (false, null, "Customer Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
