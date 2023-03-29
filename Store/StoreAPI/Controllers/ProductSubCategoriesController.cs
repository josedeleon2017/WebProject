using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSubCategoriesController : ControllerBase
    {
        private readonly StoreContext _context;

        public ProductSubCategoriesController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/ProductSubCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mProductSubCategory>>> GetProductSubCategories()
        {
            if (_context.ProductSubCategories == null)
            {
                return NotFound();
            }
            var listResult = new List<mProductSubCategory>();
            await _context.ProductSubCategories.ForEachAsync(x => listResult.Add(new mProductSubCategory()
            {
                ProductSubCategoryId = x.ProductSubCategoryId,
                ProductCategoryId = x.ProductCategoryId,
                Name = x.Name
            }));
            return listResult;
        }

        // GET: api/ProductSubCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mProductSubCategory>> GetProductSubCategory(int id)
        {
            if (_context.ProductSubCategories == null)
            {
                return NotFound();
            }
            var productSubCategory = await _context.ProductSubCategories.FindAsync(id);

            if (productSubCategory == null)
            {
                return NotFound();
            }

            var result = new mProductSubCategory()
            {
                ProductSubCategoryId = productSubCategory.ProductSubCategoryId,
                ProductCategoryId = productSubCategory.ProductCategoryId,
                Name = productSubCategory.Name
            };
            return result;
        }

        // PUT: api/ProductSubCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductSubCategory(int id, ProductSubCategory productSubCategory)
        {
            if (id != productSubCategory.ProductSubCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(productSubCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductSubCategoryExists(id))
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

        // POST: api/ProductSubCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mProductSubCategory>> PostProductSubCategory(mProductSubCategory productSubCategory)
        {
            if (_context.ProductSubCategories == null)
            {
                return Problem("Entity set 'StoreContext.ProductSubCategories'  is null.");
            }
            var productSubCategoryToInsert = new ProductSubCategory() { ProductCategoryId = productSubCategory.ProductCategoryId, Name = productSubCategory.Name };
            _context.ProductSubCategories.Add(productSubCategoryToInsert);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductSubCategory", new { id = productSubCategory.ProductCategoryId }, productSubCategory);
        }

        // DELETE: api/ProductSubCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductSubCategory(int id)
        {
            if (_context.ProductSubCategories == null)
            {
                return NotFound();
            }
            var productSubCategory = await _context.ProductSubCategories.FindAsync(id);
            if (productSubCategory == null)
            {
                return NotFound();
            }

            _context.ProductSubCategories.Remove(productSubCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductSubCategoryExists(int id)
        {
            return (_context.ProductSubCategories?.Any(e => e.ProductSubCategoryId == id)).GetValueOrDefault();
        }
    }
}
