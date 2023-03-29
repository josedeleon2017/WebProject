using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClasificacionPeliculas.Models;
using System.Drawing;

namespace ClasificacionPeliculas.Controllers
{
    public class PersonalInformationsController : Controller
    {
        private readonly MoviesContext _context;

        public PersonalInformationsController()
        {
            _context = new MoviesContext();
        }

        // GET: PersonalInformations
        public async Task<IActionResult> Index()
        {
            var moviesContext = _context.PersonalInformations.Include(p => p.GeonameidCityNavigation);
            return View(await moviesContext.ToListAsync());            
        }

        // GET: PersonalInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PersonalInformations == null)
            {
                return NotFound();
            }

            var personalInformation = await _context.PersonalInformations
                .Include(p => p.GeonameidCityNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personalInformation == null)
            {
                return NotFound();
            }

            return View(personalInformation);
        }

        // GET: PersonalInformations/Create
        public IActionResult Create()
        {
            ViewData["GeonameidCountry"] = new SelectList(_context.Countries, "Geonameid", "Name");
            return View();
        }

        // POST: PersonalInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GeonameidCity,Name,DateOfBirth,Email,PhoneNumber,Address")] PersonalInformation personalInformation)
        {
            if (ModelState.ErrorCount == 1)
            {
                _context.Add(personalInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //Relleno con el primer dropdown el país
            ViewData["GeonameidCountry"] = new SelectList(_context.Countries, "Geonameid", "Name");
            return View(personalInformation);
        }

        [HttpPost]
        public JsonResult GetRegions(string CountryGeonameId)
        {
            var regions = _context.Regions.Where(x => x.GeonameidCountry == Convert.ToInt64(CountryGeonameId));
            return Json(new SelectList(regions,"Geonameid", "Name2"));            
        }
        [HttpPost]
        public JsonResult GetCities(string RegionGeonameId)
        {
            var cities = _context.Cities.Where(x => x.GeonameidRegion == Convert.ToInt64(RegionGeonameId));
            return Json(new SelectList(cities, "Geonameid", "Name"));
        }

        // GET: PersonalInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PersonalInformations == null)
            {
                return NotFound();
            }

            var personalInformation = await _context.PersonalInformations.FindAsync(id);
            if (personalInformation == null)
            {
                return NotFound();
            }
            ViewData["GeonameidCountry"] = new SelectList(_context.Countries, "Geonameid", "Name");
            return View(personalInformation);
        }

        // POST: PersonalInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GeonameidCity,Name,DateOfBirth,Email,PhoneNumber,Address")] PersonalInformation personalInformation)
        {
            if (id != personalInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.ErrorCount == 1)
            {
                try
                {
                    _context.Update(personalInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalInformationExists(personalInformation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GeonameidCountry"] = new SelectList(_context.Countries, "Geonameid", "Name");
            return View(personalInformation);
        }

        // GET: PersonalInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PersonalInformations == null)
            {
                return NotFound();
            }

            var personalInformation = await _context.PersonalInformations
                .Include(p => p.GeonameidCityNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personalInformation == null)
            {
                return NotFound();
            }

            return View(personalInformation);
        }

        // POST: PersonalInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PersonalInformations == null)
            {
                return Problem("Entity set 'MoviesContext.PersonalInformations'  is null.");
            }
            var personalInformation = await _context.PersonalInformations.FindAsync(id);
            if (personalInformation != null)
            {
                _context.PersonalInformations.Remove(personalInformation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalInformationExists(int id)
        {
          return (_context.PersonalInformations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
    }
}
