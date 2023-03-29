using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClasificacionPeliculas.Models;
using ClasificacionPeliculasModel;
using Microsoft.IdentityModel.Tokens;

namespace ClasificacionPeliculas.Controllers
{
    public class VotesController : Controller
    {
        private readonly MoviesContext _context;

        public VotesController()
        {
            _context = new MoviesContext();
        }

        // GET: Votes
        public async Task<IActionResult> Index()
        {
            var moviesContext = _context.Votes.Include(v => v.Movies).Include(v => v.Pi);
            return View(await moviesContext.ToListAsync());
        }

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Votes == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Movies)
                .Include(v => v.Pi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // GET: Votes/Create
        public IActionResult Create()
        {
            ViewData["MoviesId"] = new SelectList(_context.Movies, "Id", "Title");
            ViewData["PiId"] = new SelectList(_context.PersonalInformations, "Id", "Name");
            return View();
        }

        // POST: Votes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PiId,MoviesId")] Vote vote)
        {
            vote.RowCreationTime = DateTime.Now;            
            if (ModelState.ErrorCount == 2)
            {
                try
                {
                    _context.Add(vote);
                    await _context.SaveChangesAsync();
                    await UpdateStatistics();
                    return RedirectToAction("Statistics","Movies");
                }
                catch (Exception)
                {
                    throw;
                }                
            }
            ViewData["MoviesId"] = new SelectList(_context.Movies, "Id", "Title", vote.MoviesId);
            ViewData["PiId"] = new SelectList(_context.PersonalInformations, "Id", "Name", vote.PiId);
            return View(vote);
        }

        private async Task UpdateStatistics()
        {
            try
            {
                var movies = _context.Movies.ToList();
                for (int i = 0; i < movies.Count(); i++)
                {
                    int votes = _context.Votes.Where(x => x.MoviesId == movies[i].Id).Count();
                    int total = _context.Votes.Count();

                    var updMovie = _context.Movies.Find(movies[i].Id);
                    if (total != 0)
                    {
                        updMovie.Rating = Convert.ToDecimal(votes) / Convert.ToDecimal(total);
                    }
                    else
                    {
                        updMovie.Rating = 0;
                    }
                    updMovie.Votes = votes;

                    _context.Update(updMovie);
                    await _context.SaveChangesAsync();
                }                
            }
            catch (Exception)
            {
                throw;
            }            
        }

        // GET: Votes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Votes == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            ViewData["MoviesId"] = new SelectList(_context.Movies, "Id", "Title", vote.MoviesId);
            ViewData["PiId"] = new SelectList(_context.PersonalInformations, "Id", "Name", vote.PiId);
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PiId,MoviesId,RowCreationTime")] Vote vote)
        {
            if (id != vote.Id)
            {
                return NotFound();
            }

            if (ModelState.ErrorCount == 2)
            {
                try
                {
                    _context.Update(vote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteExists(vote.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                await UpdateStatistics();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MoviesId"] = new SelectList(_context.Movies, "Id", "Title", vote.MoviesId);
            ViewData["PiId"] = new SelectList(_context.PersonalInformations, "Id", "Name", vote.PiId);
            return View(vote);
        }

        // GET: Votes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Votes == null)
            {
                return NotFound();
            }

            var vote = await _context.Votes
                .Include(v => v.Movies)
                .Include(v => v.Pi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // POST: Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Votes == null)
            {
                return Problem("Entity set 'MoviesContext.Votes'  is null.");
            }
            var vote = await _context.Votes.FindAsync(id);
            if (vote != null)
            {
                _context.Votes.Remove(vote);
            }
            
            await _context.SaveChangesAsync();
            await UpdateStatistics();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteExists(int id)
        {
          return (_context.Votes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
