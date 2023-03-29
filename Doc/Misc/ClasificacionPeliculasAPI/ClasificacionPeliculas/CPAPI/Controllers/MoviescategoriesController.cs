using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CPAPI.Models;

namespace CPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviescategoriesController : ControllerBase
    {
        private readonly MoviesContext _context;

        public MoviescategoriesController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/Moviescategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moviescategory>>> GetMoviescategories()
        {
          if (_context.Moviescategories == null)
          {
              return NotFound();
          }
            return await _context.Moviescategories.ToListAsync();
        }

        // GET: api/Moviescategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Moviescategory>> GetMoviescategory(int id)
        {
          if (_context.Moviescategories == null)
          {
              return NotFound();
          }
            var moviescategory = await _context.Moviescategories.FindAsync(id);

            if (moviescategory == null)
            {
                return NotFound();
            }

            return moviescategory;
        }

        // PUT: api/Moviescategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoviescategory(int id, Moviescategory moviescategory)
        {
            if (id != moviescategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(moviescategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoviescategoryExists(id))
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

        // POST: api/Moviescategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Moviescategory>> PostMoviescategory(Moviescategory moviescategory)
        {
          if (_context.Moviescategories == null)
          {
              return Problem("Entity set 'MoviesContext.Moviescategories'  is null.");
          }
            _context.Moviescategories.Add(moviescategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMoviescategory", new { id = moviescategory.Id }, moviescategory);
        }

        // DELETE: api/Moviescategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoviescategory(int id)
        {
            if (_context.Moviescategories == null)
            {
                return NotFound();
            }
            var moviescategory = await _context.Moviescategories.FindAsync(id);
            if (moviescategory == null)
            {
                return NotFound();
            }

            _context.Moviescategories.Remove(moviescategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MoviescategoryExists(int id)
        {
            return (_context.Moviescategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
