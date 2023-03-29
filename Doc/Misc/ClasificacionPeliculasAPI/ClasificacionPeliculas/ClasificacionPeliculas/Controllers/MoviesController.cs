using ClasificacionPeliculas.Calls;
using ClasificacionPeliculas.Models;
//using ClasificacionPeliculasModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClasificacionPeliculas.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesContext _context;

        public MoviesController()
        {
            _context = new MoviesContext();
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            IEnumerable<Movie> result = MoviesCalls.GetMovies().Result;
            return result != null ?
                        View(result) :
                        Problem("Entity set 'MoviesContext.Movies'  is null.");
        }

        public async Task<IActionResult> Statistics()
        {
            var result = MoviesCalls.GetMovies().Result;
            return result != null ?
                        View(result.OrderBy(x => x.Votes)) :
                        Problem("Entity set 'MoviesContext.Movies'  is null.");
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var movie = MoviesCalls.GetMovie((int)id).Result;
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Duration,Director,Actors,Plot,Rating,Votes,PosterUrl,ImdbId")] ClasificacionPeliculasModel.Movie movie)
        {
            if (ModelState.IsValid)
            {
                var insertMovie = new Movie()
                {
                    Title = movie.Title,
                    ReleaseDate = movie.ReleaseDate,
                    Duration = movie.Duration,
                    Director = movie.Director,
                    Actors = movie.Actors,
                    Plot = movie.Plot,
                    Rating = (decimal)movie.Rating,
                    Votes = (int)movie.Votes,
                    PosterUrl = movie.PosterUrl,
                    ImdbId = movie.PosterUrl
                };
                var result = MoviesCalls.InsertMovie(insertMovie);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }
            var movie = await MoviesCalls.GetMovie((int)id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Duration,Director,Actors,Plot,Rating,Votes,PosterUrl,ImdbId")] ClasificacionPeliculasModel.Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updMovie = new Models.Movie()
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        ReleaseDate = movie.ReleaseDate,
                        Duration = movie.Duration,
                        Director = movie.Director,
                        Actors = movie.Actors,
                        Plot = movie.Plot,
                        Rating = (decimal)movie.Rating,
                        Votes = (int)movie.Votes,
                        PosterUrl = movie.PosterUrl,
                        ImdbId = movie.PosterUrl
                    };
                    var result = MoviesCalls.UpdateMovie(updMovie).Result;
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'MoviesContext.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public JsonResult GetMovieJson()
        {
            MoviesContext _moviesContext = new MoviesContext();
            int movieId = Convert.ToInt32(HttpContext.Request.Form["movieId"].FirstOrDefault().ToString());

            ClasificacionPeliculasModel.Movie movie = (from m in _moviesContext.Movies
                                                       where m.Id == movieId
                                                       select new ClasificacionPeliculasModel.Movie
                                                       {
                                                           Title = m.Title,
                                                           ReleaseDate = m.ReleaseDate,
                                                           Duration = m.Duration,
                                                           Director = m.Director,
                                                           Actors = m.Actors

                                                       }).FirstOrDefault();
            var jsonresult = new { movie = movie };

            return Json(jsonresult);
        }

        [HttpGet]
        public JsonResult GetRatings()
        {
            var movies = _context.Movies.ToList();
            List<RatingElement> r = new List<RatingElement>();
            for (int i = 0; i < movies.Count; i++)
            {
                r.Add(new RatingElement() { Value = movies[i].Votes, Name = movies[i].Title });
            }
            return Json(r);
        }
    }
}
