using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using Store.Models;
using StoreModels.Models;
using StoreMVC.APIServices;
using StoreMVC.Models;
using System.Diagnostics;

namespace StoreMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
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
            using (var service = new ProductServices())
            {
                content.Products = await service.GetProductsUser();
            }
            using (var service = new CrudService<int, mProductCategory>("ProductCategories"))
            {
                content.Categories = await service.GetAll();
            }
            return View(content);
        }

        [Authorize]
        public async Task<IActionResult> Detail(int id)
        {
            //Modificar para que use itemProduct
            using (var service = new ProductServices())
            {
                return View(await service.GetOneProductUser(id));
            }
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