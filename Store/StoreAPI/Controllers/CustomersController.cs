using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;
using StoreModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomersController : ControllerBase
    {
        private readonly StoreContext _context;

        public CustomersController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mCustomer>>> GetCustomers()
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var listResult = new List<mCustomer>();
            await _context.Customers.ForEachAsync(x => listResult.Add(new mCustomer()
            {
                CustomerId = x.CustomerId,
                AddressId = x.AddressId,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                Password = x.Password,
                EmailPromotion = x.EmailPromotion
            }));
            return listResult;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mCustomer>> GetCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var result = new mCustomer()
            {
                CustomerId = customer.CustomerId,
                AddressId = customer.AddressId,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                Password = customer.Password,
                EmailPromotion = customer.EmailPromotion
            };
            return result;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, mCustomer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            var customerBase = _context.Customers.FirstOrDefault(x => x.CustomerId == id);
            if (customerBase != null)
            {
                customerBase.FirstName = customer.FirstName;
                customerBase.MiddleName = customer.MiddleName;
                customerBase.LastName = customer.LastName;
                customerBase.EmailAddress = customer.EmailAddress;
                customerBase.Password = customer.Password;
                customerBase.PhoneNumber = customer.PhoneNumber;
                customerBase.EmailPromotion = customer.EmailPromotion;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mCustomer>> PostCustomer(mCustomer customer)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'StoreContext.Customers'  is null.");
            }
            var customerToInsert = new Customer()
            {
                AddressId = customer.AddressId,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                Password = customer.Password,
                PhoneNumber = customer.PhoneNumber,
                EmailPromotion = customer.EmailPromotion,
            };
            _context.Customers.Add(customerToInsert);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
