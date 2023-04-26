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
using System.Net;
using System.Threading.Tasks;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SalesOrderHeadersController : ControllerBase
    {
        private readonly StoreContext _context;

        public SalesOrderHeadersController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/SalesOrderHeaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mSalesOrderHeader>>> GetSalesOrderHeaders()
        {
            if (_context.SalesOrderHeaders == null)
            {
                return NotFound();
            }
            var listResult = new List<mSalesOrderHeader>();
            await _context.SalesOrderHeaders.ForEachAsync(x => listResult.Add(new mSalesOrderHeader()
            {
                SalesOrderId = x.SalesOrderId,
                CustomerId = x.CustomerId,
                ShipToAddressId = x.ShipToAddressId,
                ShipToMethodId = x.ShipToMethodId,
                OrderDate = x.OrderDate,
                DueDate = x.DueDate,
                ShipDate = x.ShipDate,
                Status = x.Status,
                CreditCardType = x.CreditCardType,
                CardNumber = x.CardNumber,
                ExpMonth = x.ExpMonth,
                ExpYear = x.ExpYear,
                CreditCardApprovalCode = x.CreditCardApprovalCode,
                SubTotal = x.SubTotal,
                TaxAmt = x.TaxAmt,
                Freight = x.Freight,
                TotalDue = x.TotalDue,
                Comment = x.Comment
            }));
            return listResult;
        }

        // GET: api/SalesOrderHeaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mSalesOrderHeader>> GetSalesOrderHeader(int id)
        {
            if (_context.SalesOrderHeaders == null)
            {
                return NotFound();
            }
            var salesOrderHeader = await _context.SalesOrderHeaders.FindAsync(id);

            if (salesOrderHeader == null)
            {
                return NotFound();
            }

            var result = new mSalesOrderHeader()
            {
                SalesOrderId = salesOrderHeader.SalesOrderId,
                CustomerId = salesOrderHeader.CustomerId,
                ShipToAddressId = salesOrderHeader.ShipToAddressId,
                ShipToMethodId = salesOrderHeader.ShipToMethodId,
                OrderDate = salesOrderHeader.OrderDate,
                DueDate = salesOrderHeader.DueDate,
                ShipDate = salesOrderHeader.ShipDate,
                Status = salesOrderHeader.Status,
                CreditCardType = salesOrderHeader.CreditCardType,
                CardNumber = salesOrderHeader.CardNumber,
                ExpMonth = salesOrderHeader.ExpMonth,
                ExpYear = salesOrderHeader.ExpYear,
                CreditCardApprovalCode = salesOrderHeader.CreditCardApprovalCode,
                SubTotal = salesOrderHeader.SubTotal,
                TaxAmt = salesOrderHeader.TaxAmt,
                Freight = salesOrderHeader.Freight,
                TotalDue = salesOrderHeader.TotalDue,
                Comment = salesOrderHeader.Comment
            };
            return result;
        }

        // PUT: api/SalesOrderHeaders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutSalesOrderHeader(int id, SalesOrderHeader salesOrderHeader)
        //{
        //    if (id != salesOrderHeader.SalesOrderId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(salesOrderHeader).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SalesOrderHeaderExists(id))
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

        // POST: api/SalesOrderHeaders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mSalesOrderHeader>> PostSalesOrderHeader(mSalesOrderHeader salesOrderHeader)
        {
            if (_context.SalesOrderHeaders == null)
            {
                return Problem("Entity set 'StoreContext.SalesOrderHeaders'  is null.");
            }
            var headerToInsert = new SalesOrderHeader()
            {
                SalesOrderId = salesOrderHeader.SalesOrderId,
                CustomerId = salesOrderHeader.CustomerId,
                ShipToAddressId = salesOrderHeader.ShipToAddressId,
                ShipToMethodId = salesOrderHeader.ShipToMethodId,
                OrderDate = salesOrderHeader.OrderDate,
                DueDate = salesOrderHeader.DueDate,
                ShipDate = salesOrderHeader.ShipDate,
                Status = salesOrderHeader.Status,
                CreditCardType = salesOrderHeader.CreditCardType,
                CardNumber = salesOrderHeader.CardNumber,
                ExpMonth = salesOrderHeader.ExpMonth,
                ExpYear = salesOrderHeader.ExpYear,
                CreditCardApprovalCode = salesOrderHeader.CreditCardApprovalCode,
                SubTotal = salesOrderHeader.SubTotal,
                TaxAmt = salesOrderHeader.TaxAmt,
                Freight = salesOrderHeader.Freight,
                TotalDue = salesOrderHeader.TotalDue,
                Comment = salesOrderHeader.Comment
            };
            _context.SalesOrderHeaders.Add(headerToInsert);
            await _context.SaveChangesAsync();
            salesOrderHeader.SalesOrderId = headerToInsert.SalesOrderId;
            return CreatedAtAction("GetSalesOrderHeader", new { id = salesOrderHeader.SalesOrderId }, salesOrderHeader);
        }

        //// DELETE: api/SalesOrderHeaders/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSalesOrderHeader(int id)
        //{
        //    if (_context.SalesOrderHeaders == null)
        //    {
        //        return NotFound();
        //    }
        //    var salesOrderHeader = await _context.SalesOrderHeaders.FindAsync(id);
        //    if (salesOrderHeader == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.SalesOrderHeaders.Remove(salesOrderHeader);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool SalesOrderHeaderExists(int id)
        {
            return (_context.SalesOrderHeaders?.Any(e => e.SalesOrderId == id)).GetValueOrDefault();
        }
    }
}
