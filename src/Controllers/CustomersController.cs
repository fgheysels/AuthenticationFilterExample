using Codit.SharedAccessKeyExample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Codit.SharedAccessKeyExample.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CustomersController : Controller
    {
        private readonly List<Customer> _customers = new List<Customer>() { new Customer(1, "Marc"), new Customer(2, "François"), new Customer(3, "Fabian") };

        [HttpGet]
        public IActionResult GetCustomers()
        {
            return Ok(_customers.ToArray());
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomer([FromRoute] int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
    }
}
