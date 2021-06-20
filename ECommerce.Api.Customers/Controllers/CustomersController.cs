using ECommerce.Api.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Controllers
{
    [ApiController]
    [Route("/api/Customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersProvider customersprovider;

        public CustomersController(ICustomersProvider customersprovider)
        {
            this.customersprovider = customersprovider;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await customersprovider.GetCustomersAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Customers);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsAsync(int id)
        {
            var result = await customersprovider.GetCustomersAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Customer);
            }
            return NotFound();
        }
    }
}
