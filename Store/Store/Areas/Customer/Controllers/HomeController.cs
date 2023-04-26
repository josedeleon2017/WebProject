using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using Store.Models;
using StoreModels.Models;
using StoreMVC.APIServices;
using StoreMVC.Models;
using System.Diagnostics;
using System.Security.Claims;

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

        public async Task<IActionResult> Detail(int id)
        {
            ProductCartItemVM cartItemVM = new ProductCartItemVM();
            cartItemVM.CartItem = new mShoppingCartItem();
            //products
            using (var service = new CrudService<int, mProduct>("Products"))
            {
                var product_result = await service.GetOne(id);
                cartItemVM.Product = product_result;
            }
            cartItemVM.CartItem.ProductId = id;
            cartItemVM.CartItem.OrderQty = 1;
            return View(cartItemVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Detail(ProductCartItemVM item)
        {
            //cartItem
            using (var service = new CrudService<int, mShoppingCartItem>("ShoppingCartItems"))
            {
                item.CartItem.CustomerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                item.CartItem.DateCreated = DateTime.Now;
                await service.Add(item.CartItem);
                TempData["success"] = "Item added to ShoppingCart!";
                return RedirectToAction("Principal", "Home", new { area = "Customer" });
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