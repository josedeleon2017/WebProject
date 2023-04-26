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
    public class StatesController : ControllerBase
    {
        private readonly StoreContext _context;

        public StatesController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/States
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mState>>> GetStates()
        {
            if (_context.States == null)
            {
                return NotFound();
            }
            var listResult = new List<mState>();
            await _context.States.ForEachAsync(x => listResult.Add(new mState()
            {
                StateId = x.StateId,
                RegionId = x.RegionId,
                Name = x.Name,
            }));
            return listResult;
        }

        // GET: api/ProductCategories/5        
        [HttpGet("{id}")]
        public async Task<ActionResult<mState>> GetState(int id)
        {
            if (_context.States == null)
            {
                return NotFound();
            }
            var state = await _context.States.FindAsync(id);

            if (state == null)
            {
                return NotFound();
            }
            var result = new mState()
            {
                StateId = state.StateId,
                RegionId = state.RegionId,
                Name = state.Name,
            };
            return result;
        }
    }
}
