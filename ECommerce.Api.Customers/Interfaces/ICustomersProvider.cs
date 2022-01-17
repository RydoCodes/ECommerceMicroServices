using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Customerdto> Customers, string ErrorMessage)> GetCustomersAsync();
        Task<(bool IsSuccess, Customerdto Customer, string ErrorMessage)> GetCustomersAsync(int id);
    }
}
