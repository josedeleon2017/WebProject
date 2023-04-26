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
            var customer = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            using (var service = new CrudService<int, mShoppingCartItem>("ShoppingCartItems"))
            {
                var existItem = await service.GetAll();
                var itemMod = existItem.FirstOrDefault(x => x.ProductId == item.CartItem.ProductId && x.CustomerId == customer);
                if (itemMod != null)
                {
                    var mod = await service.GetOne(itemMod.ShoppingCartItemId);
                    mod.OrderQty += item.CartItem.OrderQty;
                    mod.DateCreated = DateTime.Now;
                    await service.Update(mod.ShoppingCartItemId, mod);
                }
                else
                {
                    item.CartItem.CustomerId = customer;
                    item.CartItem.DateCreated = DateTime.Now;
                    await service.Add(item.CartItem);
                }
                TempData["success"] = "Item added to ShoppingCart!";
                return RedirectToAction("Principal", "Home", new { area = "Customer" });
            }
        }

        [Authorize]
        public async Task<IActionResult> ShoppingCart()
        {
            List<ProductCartItemVM> pca = new List<ProductCartItemVM>();
            decimal total = 0;
            var customer = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            //get all items
            using (var service = new CrudService<int, mShoppingCartItem>("ShoppingCartItems"))
            {
                var items = await service.GetAll();
                var itemList = items.Where(x => x.CustomerId == customer).ToList();

                //get all products
                for (int i = 0; i < itemList.Count; i++)
                {
                    using (var service2 = new CrudService<int, mProduct>("Products"))
                    {
                        var product = await service2.GetOne(itemList.ElementAt(i).ProductId);
                        ProductCartItemVM tmp = new ProductCartItemVM();
                        tmp.CartItem = itemList.ElementAt(i);
                        tmp.Product = product;
                        total += product.ListPrice * tmp.CartItem.OrderQty;
                        tmp.SubTotal = product.ListPrice * tmp.CartItem.OrderQty;
                        pca.Add(tmp);
                    }
                }
            }
            ViewBag.Total = total;
            return View(pca);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            using (var service = new CrudService<int, mShoppingCartItem>("ShoppingCartItems"))
            {
                TempData["success"] = "CartItem deleted succesfully!";
                await service.Delete(id);
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true });
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