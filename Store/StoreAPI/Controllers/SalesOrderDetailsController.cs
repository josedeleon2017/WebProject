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
    public class SalesOrderDetailsController : ControllerBase
    {
        private readonly StoreContext _context;

        public SalesOrderDetailsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/SalesOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mSalesOrderDetail>>> GetSalesOrderDetails()
        {
            if (_context.SalesOrderDetails == null)
            {
                return NotFound();
            }
            var listResult = new List<mSalesOrderDetail>();
            await _context.SalesOrderDetails.ForEachAsync(x => listResult.Add(new mSalesOrderDetail()
            {
                SalesOrderId = x.SalesOrderId,
                SalesOrderDetailId = x.SalesOrderDetailId,
                ProductId = x.ProductId,
                OrderQty = x.OrderQty,
                UnitPrice = x.UnitPrice,
                UnitPriceDiscount = x.UnitPriceDiscount,
                LineTotal = x.LineTotal
            }));
            return listResult;
        }

        // GET: api/SalesOrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mSalesOrderDetail>> GetSalesOrderDetail(int id)
        {
            if (_context.SalesOrderDetails == null)
            {
                return NotFound();
            }
            var salesOrderDetail = await _context.SalesOrderDetails.FindAsync(id);

            if (salesOrderDetail == null)
            {
                return NotFound();
            }

            var result = new mSalesOrderDetail()
            {
                SalesOrderId = salesOrderDetail.SalesOrderId,
                SalesOrderDetailId = salesOrderDetail.SalesOrderDetailId,
                ProductId = salesOrderDetail.ProductId,
                OrderQty = salesOrderDetail.OrderQty,
                UnitPrice = salesOrderDetail.UnitPrice,
                UnitPriceDiscount = salesOrderDetail.UnitPriceDiscount,
                LineTotal = salesOrderDetail.LineTotal
            };
            return result;
        }

        //// PUT: api/SalesOrderDetails/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSalesOrderDetail(int id, mSalesOrderDetail salesOrderDetail)
        //{
        //    if (id != salesOrderDetail.SalesOrderId)
        //    {
        //        return BadRequest();
        //    }

        //    var OdBase = _context.SalesOrderDetails.FirstOrDefault(x => x. == id);
        //    if (OdBase != null)
        //    {
        //        OdBase.Name = productCategory.Name;
        //    }

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SalesOrderDetailExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/SalesOrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mSalesOrderDetail>> PostSalesOrderDetail(mSalesOrderDetail salesOrderDetail)
        {
            if (_context.SalesOrderDetails == null)
            {
                return Problem("Entity set 'StoreContext.SalesOrderDetails'  is null.");
            }
            var detailToInsert = new SalesOrderDetail()
            {
                SalesOrderId = salesOrderDetail.SalesOrderId,
                SalesOrderDetailId = salesOrderDetail.SalesOrderDetailId,
                ProductId = salesOrderDetail.ProductId,
                OrderQty = salesOrderDetail.OrderQty,
                UnitPrice = salesOrderDetail.UnitPrice,
                UnitPriceDiscount = salesOrderDetail.UnitPriceDiscount,
                LineTotal = salesOrderDetail.LineTotal
            };
            _context.SalesOrderDetails.Add(detailToInsert);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesOrderDetail", new { id = salesOrderDetail.SalesOrderId }, salesOrderDetail);
        }

        //// DELETE: api/SalesOrderDetails/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSalesOrderDetail(int id)
        //{
        //    if (_context.SalesOrderDetails == null)
        //    {
        //        return NotFound();
        //    }
        //    var salesOrderDetail = await _context.SalesOrderDetails.FindAsync(id);
        //    if (salesOrderDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.SalesOrderDetails.Remove(salesOrderDetail);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool SalesOrderDetailExists(int id)
        {
            return (_context.SalesOrderDetails?.Any(e => e.SalesOrderId == id)).GetValueOrDefault();
        }
    }
}
