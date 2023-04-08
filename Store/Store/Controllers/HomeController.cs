using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using Store.Models;
using StoreModels.Models;
using StoreMVC.Models;
using System.Diagnostics;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Principal()
        {
            var content = new ProductsCategoriesVM();
            using (var service = new CrudService<int, mProduct>("Products"))
            {
                content.Products = await service.GetAll();
            }
            using (var service = new CrudService<int, mProductCategory>("ProductCategories"))
            {
                content.Categories = await service.GetAll();
            }
            return View(content);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}