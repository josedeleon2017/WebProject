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
    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;

        public MoviesController()
        {
            _context = new MoviesContext();
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            return await _context.Movies.ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult<Movie>> PutMovie(Movie movie)
        {
            Movie cMovie = _context.Movies.FirstOrDefault(s => s.Id == movie.Id);
            cMovie.Actors = movie.Actors;
            cMovie.Director = movie.Director;
            cMovie.Duration = movie.Duration;
            cMovie.ImdbId = movie.ImdbId;
            cMovie.PosterUrl = movie.PosterUrl;
            cMovie.Title = movie.Title;
            cMovie.ReleaseDate = movie.ReleaseDate;
            cMovie.Plot = movie.Plot;
            cMovie.Rating = movie.Rating;
            cMovie.Votes = movie.Votes;

            try
            {
                _context.Movies.Update(cMovie);
                await _context.SaveChangesAsync();
                return cMovie;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            try
            {
                if (_context.Movies == null)
                {
                    return Problem("Entity set 'MoviesContext.Movies'  is null.");
                }
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
            }
            catch (Exception e)
            {
                string error = e.Message;
                throw new Exception();
            }
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
