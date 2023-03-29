using CPAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalInformationsController : ControllerBase
    {
        private readonly MoviesContext _context;

        public PersonalInformationsController(MoviesContext context)
        {
            _context = new MoviesContext();
        }

        // GET: api/PersonalInformations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalInformation>>> GetPersonalInformations()
        {
            if (_context.PersonalInformations == null)
            {
                return NotFound();
            }
            return await _context.PersonalInformations.ToListAsync();
        }

        // GET: api/PersonalInformations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonalInformation>> GetPersonalInformation(int id)
        {
            if (_context.PersonalInformations == null)
            {
                return NotFound();
            }
            var personalInformation = await _context.PersonalInformations.FindAsync(id);

            if (personalInformation == null)
            {
                return NotFound();
            }

            return personalInformation;
        }

        // PUT: api/PersonalInformations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonalInformation(int id, PersonalInformation personalInformation)
        {
            if (id != personalInformation.Id)
            {
                return BadRequest();
            }

            _context.Entry(personalInformation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalInformationExists(id))
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

        // POST: api/PersonalInformations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PersonalInformation>> PostPersonalInformation(PersonalInformation personalInformation)
        {
            if (_context.PersonalInformations == null)
            {
                return Problem("Entity set 'MoviesContext.PersonalInformations'  is null.");
            }
            _context.PersonalInformations.Add(personalInformation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonalInformation", new { id = personalInformation.Id }, personalInformation);
        }

        // DELETE: api/PersonalInformations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonalInformation(int id)
        {
            if (_context.PersonalInformations == null)
            {
                return NotFound();
            }
            var personalInformation = await _context.PersonalInformations.FindAsync(id);
            if (personalInformation == null)
            {
                return NotFound();
            }

            _context.PersonalInformations.Remove(personalInformation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonalInformationExists(int id)
        {
            return (_context.PersonalInformations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
