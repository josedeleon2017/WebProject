using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
    public class ShoppingCartItemsController : ControllerBase
    {
        private readonly StoreContext _context;

        public ShoppingCartItemsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingCartItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mShoppingCartItem>>> GetShoppingCartItems()
        {
            if (_context.ShoppingCartItems == null)
            {
                return NotFound();
            }
            var listResult = new List<mShoppingCartItem>();
            await _context.ShoppingCartItems.ForEachAsync(x => listResult.Add(new mShoppingCartItem()
            {
                ShoppingCartItemId = x.ShoppingCartItemId,
                CustomerId = x.CustomerId,
                ProductId = x.ProductId,
                DateCreated = x.DateCreated,
                OrderQty = x.OrderQty
            }));
            return listResult;
        }

        // GET: api/ShoppingCartItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mShoppingCartItem>> GetShoppingCartItem(int id)
        {
            if (_context.ShoppingCartItems == null)
            {
                return NotFound();
            }
            var item = await _context.ShoppingCartItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            var result = new mShoppingCartItem()
            {
                ShoppingCartItemId = item.ShoppingCartItemId,
                CustomerId = item.CustomerId,
                ProductId = item.ProductId,
                DateCreated = item.DateCreated,
                OrderQty = item.OrderQty
            };
            return result;
        }

        // PUT: api/ShoppingCartItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingCartItem(int id, mShoppingCartItem item)
        {
            if (id != item.ShoppingCartItemId)
            {
                return BadRequest();
            }

            var itemBase = _context.ShoppingCartItems.FirstOrDefault(x => x.ShoppingCartItemId == id);
            if (itemBase != null)
            {
                itemBase.ShoppingCartItemId = item.ShoppingCartItemId;
                itemBase.CustomerId = item.CustomerId;
                itemBase.ProductId = item.ProductId;
                itemBase.DateCreated = item.DateCreated;
                itemBase.OrderQty = item.OrderQty;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingCartItemExists(id))
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

        // POST: api/ShoppingCartItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mShoppingCartItem>> PostShoppingCartItem(mShoppingCartItem shoppingCartItem)
        {
            if (_context.ShoppingCartItems == null)
            {
                return Problem("Entity set 'StoreContext.ShoppingCartItems'  is null.");
            }
            var itemToInsert = new ShoppingCartItem()
            {
                ShoppingCartItemId = shoppingCartItem.ShoppingCartItemId,
                CustomerId = shoppingCartItem.CustomerId,
                ProductId = shoppingCartItem.ProductId,
                DateCreated = shoppingCartItem.DateCreated,
                OrderQty = shoppingCartItem.OrderQty
            };
            _context.ShoppingCartItems.Add(itemToInsert);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingCartItem", new { id = shoppingCartItem.ShoppingCartItemId }, shoppingCartItem);
        }

        // DELETE: api/ShoppingCartItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCartItem(int id)
        {
            if (_context.ShoppingCartItems == null)
            {
                return NotFound();
            }
            var shoppingCartItem = await _context.ShoppingCartItems.FindAsync(id);
            if (shoppingCartItem == null)
            {
                return NotFound();
            }

            _context.ShoppingCartItems.Remove(shoppingCartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShoppingCartItemExists(int id)
        {
            return (_context.ShoppingCartItems?.Any(e => e.ShoppingCartItemId == id)).GetValueOrDefault();
        }
    }
}
