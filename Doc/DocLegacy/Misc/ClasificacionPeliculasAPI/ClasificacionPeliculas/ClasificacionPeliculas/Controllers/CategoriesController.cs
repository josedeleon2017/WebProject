using ClasificacionPeliculas.Calls;
using ClasificacionPeliculas.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClasificacionPeliculas.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            var result = CategoriesCalls.GetCategories().Result;
            IEnumerable<ClasificacionPeliculasModel.Category> categories = (from c in result
                                                                            select new ClasificacionPeliculasModel.Category
                                                                            {
                                                                                Id = c.Id,
                                                                                Name = c.Name
                                                                            }).OrderBy(s => s.Name).ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string Name)
        {
            Category category = new Category
            {
                Name = Name
            };
            var result = CategoriesCalls.InsertCategory(category).Result;
            ClasificacionPeliculasModel.Category categoryResult = new ClasificacionPeliculasModel.Category
            {
                Name = Name,
                Id = result.Id,
                Result = new ClasificacionPeliculasModel.GeneralResult
                {
                    Result = true
                }
            };
            ViewBag.Resultado = true;
            return View(categoryResult);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category category = CategoriesCalls.GetCategory(id).Result;
            ClasificacionPeliculasModel.Category categoryResult = new ClasificacionPeliculasModel.Category
            {
                Id = category.Id,
                Name = category.Name,
            };
            return View(categoryResult);
        }

        [HttpPost]
        public IActionResult Edit(int Id, string Name)
        {
            Category cat = new Category() { Id = Id, Name = Name };
            var category = CategoriesCalls.UpdateCategory(cat).Result;
            ClasificacionPeliculasModel.Category categoryResult = new ClasificacionPeliculasModel.Category
            {
                Name = Name,
                Id = category.Id,
                Result = new ClasificacionPeliculasModel.GeneralResult
                {
                    Result = true
                }
            };
            ViewBag.Resultado = true;
            return View(categoryResult);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Category category = CategoriesCalls.GetCategory(id).Result;
            ClasificacionPeliculasModel.Category categoryResult = new ClasificacionPeliculasModel.Category
            {
                Id = category.Id,
                Name = category.Name,
            };
            return View(categoryResult);
        }
    }
}
