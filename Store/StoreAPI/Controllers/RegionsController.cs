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
    public class RegionsController : ControllerBase
    {
        private readonly StoreContext _context;

        public RegionsController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Regions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mRegion>>> GetRegions()
        {
            if (_context.Regions == null)
            {
                return NotFound();
            }
            var listResult = new List<mRegion>();
            await _context.Regions.ForEachAsync(x => listResult.Add(new mRegion()
            {
                RegionId = x.RegionId,
                Name = x.Name,
            }));
            return listResult;
        }

    }
}
