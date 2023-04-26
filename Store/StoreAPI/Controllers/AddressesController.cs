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
    public class AddressesController : ControllerBase
    {
        private readonly StoreContext _context;

        public AddressesController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mAddress>>> GetAddresses()
        {
            if (_context.Addresses == null)
            {
                return NotFound();
            }
            var listResult = new List<mAddress>();
            await _context.Addresses.ForEachAsync(x => listResult.Add(new mAddress()
            {
                AddressId = x.AddressId,
                StateId = x.StateId,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                PostalCode = x.PostalCode,
            }));
            return listResult;
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mAddress>> GetAddress(int id)
        {
            if (_context.Addresses == null)
            {
                return NotFound();
            }
            var address = await _context.Addresses.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            var result = new mAddress()
            {
                AddressId = address.AddressId,
                StateId = address.StateId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PostalCode = address.PostalCode,
            };
            return result;
        }

        // PUT: api/Addresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, mAddress address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            var addressBase = _context.Addresses.FirstOrDefault(x => x.AddressId == id);
            if (addressBase != null)
            {
                addressBase.StateId = address.StateId;
                addressBase.AddressLine1 = address.AddressLine1;
                addressBase.AddressLine2 = address.AddressLine2;
                addressBase.PostalCode = address.PostalCode;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        // POST: api/Addresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mAddress>> PostAddress(mAddress address)
        {
            if (_context.Addresses == null)
            {
                return Problem("Entity set 'StoreContext.Addresses'  is null.");
            }
            var addressToInsert = new Address()
            {
                StateId = address.StateId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PostalCode = address.PostalCode,
            };
            _context.Addresses.Add(addressToInsert);
            await _context.SaveChangesAsync();
            address.AddressId = addressToInsert.AddressId;
            return CreatedAtAction("GetAddress", new { id = addressToInsert.AddressId }, address);
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (_context.Addresses == null)
            {
                return NotFound();
            }
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return (_context.Addresses?.Any(e => e.AddressId == id)).GetValueOrDefault();
        }
    }
}
