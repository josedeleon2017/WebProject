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
    public class ProductCategoriesController : ControllerBase
    {
        private readonly StoreContext _context;

        public ProductCategoriesController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/ProductCategories        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mProductCategory>>> GetProductCategories()
        {
            if (_context.ProductCategories == null)
            {
                return NotFound();
            }
            var listResult = new List<mProductCategory>();
            await _context.ProductCategories.ForEachAsync(x => listResult.Add(new mProductCategory() { ProductCategoryId = x.ProductCategoryId, Name = x.Name }));
            return listResult;
        }

        // GET: api/ProductCategories/5        
        [HttpGet("{id}")]
        public async Task<ActionResult<mProductCategory>> GetProductCategory(int id)
        {
            if (_context.ProductCategories == null)
            {
                return NotFound();
            }
            var productCategory = await _context.ProductCategories.FindAsync(id);

            if (productCategory == null)
            {
                return NotFound();
            }
            var result = new mProductCategory() { ProductCategoryId = productCategory.ProductCategoryId, Name = productCategory.Name };
            return result;
        }

        // PUT: api/ProductCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, mProductCategory productCategory)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return BadRequest();
            }
            var categoryBase = _context.ProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
            if (categoryBase != null)
            {
                categoryBase.Name = productCategory.Name;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
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

        // POST: api/ProductCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754        
        [HttpPost]
        public async Task<ActionResult<mProductCategory>> PostProductCategory(mProductCategory productCategory)
        {
            if (_context.ProductCategories == null)
            {
                return Problem("Entity set 'StoreContext.ProductCategories'  is null.");
            }
            var productCategoryToInsert = new ProductCategory() { Name = productCategory.Name };
            _context.ProductCategories.Add(productCategoryToInsert);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategory);
        }

        // DELETE: api/ProductCategories/5        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            if (_context.ProductCategories == null)
            {
                return NotFound();
            }
            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductCategoryExists(int id)
        {
            return (_context.ProductCategories?.Any(e => e.ProductCategoryId == id)).GetValueOrDefault();
        }
    }
}