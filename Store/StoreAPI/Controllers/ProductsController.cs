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
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mProduct>>> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var listResult = new List<mProduct>();
            await _context.Products.ForEachAsync(x => listResult.Add(new mProduct()
            {
                ProductId = x.ProductId,
                ProductSubCategoryId = x.ProductSubCategoryId,
                Name = x.Name,
                Description = x.Description,
                Specifications = x.Specifications,
                StandarCost = x.StandarCost,
                ListPrice = x.ListPrice,
                SellEndDate = x.SellEndDate,
                SellStartDate = x.SellStartDate,
                ActiveFlag = x.ActiveFlag,
                LowStock = x.LowStock,
                ImagePath = x.ImagePath,
            }));
            return listResult;
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<mProduct>>> GetProductsUser()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var listResult = new List<mProduct>();
            await _context.Products.ForEachAsync(x => listResult.Add(new mProduct()
            {
                ProductId = x.ProductId,
                Name = x.Name,
                ListPrice = x.ListPrice,
                ImagePath = x.ImagePath,
                ActiveFlag = x.ActiveFlag
            }));
            return listResult.Where(x => Convert.ToBoolean(x.ActiveFlag)).ToList();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mProduct>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var result = new mProduct()
            {
                ProductId = product.ProductId,
                ProductSubCategoryId = product.ProductSubCategoryId,
                Name = product.Name,
                Description = product.Description,
                Specifications = product.Specifications,
                StandarCost = product.StandarCost,
                ListPrice = product.ListPrice,
                SellEndDate = product.SellEndDate,
                SellStartDate = product.SellStartDate,
                ActiveFlag = product.ActiveFlag,
                LowStock = product.LowStock,
                ImagePath = product.ImagePath,
            };
            return result;
        }

        // GET: api/Products/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<mProduct>> GetProductUser(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var result = new mProduct()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Specifications = product.Specifications,
                ListPrice = product.ListPrice,
                ImagePath = product.ImagePath,
            };
            return result;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, mProduct product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            var productBase = _context.Products.FirstOrDefault(x => x.ProductId == id);
            if (productBase != null)
            {
                productBase.ProductSubCategoryId = product.ProductSubCategoryId;
                productBase.Name = product.Name;
                productBase.Description = product.Description;
                productBase.Specifications = product.Specifications;
                productBase.StandarCost = product.StandarCost;
                productBase.ListPrice = product.ListPrice;
                productBase.SellEndDate = product.SellEndDate;
                productBase.SellStartDate = product.SellStartDate;
                productBase.ImagePath = product.ImagePath;
                productBase.LowStock = product.LowStock;
                productBase.ActiveFlag = product.ActiveFlag;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(mProduct product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'StoreContext.Products'  is null.");
            }
            var productToInsert = new Product()
            {
                ProductSubCategoryId = product.ProductSubCategoryId,
                Name = product.Name,
                Description = product.Description,
                Specifications = product.Specifications,
                StandarCost = product.StandarCost,
                ListPrice = product.ListPrice,
                SellEndDate = product.SellEndDate,
                SellStartDate = product.SellStartDate,
                ActiveFlag = product.ActiveFlag,
                LowStock = product.LowStock,
                ImagePath = product.ImagePath
            };
            _context.Products.Add(productToInsert);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
