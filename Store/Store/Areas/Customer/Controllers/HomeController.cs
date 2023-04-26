using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
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
            var role = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value);
            if (role == 1 || role == 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

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
            var role = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value);
            if (role == 1 || role == 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

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


        public async Task<IActionResult> GenerateOrder()
        {
            //Global result
            var globalResult = new OrderVM();
            globalResult.OrderHeader = new mSalesOrderHeader();
            //-----------------------------Items of the user
            List<ProductCartItemVM> pca = new List<ProductCartItemVM>();
            decimal total = 0;
            var customer = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            using (var service = new CrudService<int, mShoppingCartItem>("ShoppingCartItems"))
            {
                var items = await service.GetAll();
                var itemList = items.Where(x => x.CustomerId == customer).ToList();

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
            globalResult.CartVM = pca;
            globalResult.OrderHeader.TotalDue = total;
            //-----------------------------Address of the user
            using (var service = new CrudService<int, mCustomer>("Customers"))
            {
                var customerId = await service.GetOne(customer);
                using (var service2 = new CrudService<int, mAddress>("Addresses"))
                {
                    var address = await service2.GetOne(customerId.AddressId);
                    globalResult.Address = address;
                }
            }
            //globalResult.OrderHeader = new mSalesOrderHeader();

            //---------------------------Format Address
            //Obtiene el Id del padre
            int RegionId;
            using (var service = new CrudService<int, mState>("States"))
            {
                var result = await service.GetOne(globalResult.Address.StateId);
                RegionId = result.RegionId;
            }

            //Obtiene todos los padres y selecciona el especifico
            using (var service = new CrudService<int, mRegion>("Regions"))
            {
                var regions = await service.GetAll();
                IEnumerable<SelectListItem> items = regions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.RegionId.ToString(),
                    Selected = x.RegionId == RegionId
                });
                ViewBag.RegionList = items;
            }
            //Obtiene todos los hijos con base al padre
            using (var service = new CrudService<int, mState>("States"))
            {
                var states = await service.GetAll();
                var subFilter = states.Where(x => x.RegionId == RegionId);
                IEnumerable<SelectListItem> items = subFilter.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.StateId.ToString(),
                    Selected = x.StateId == globalResult.Address.StateId
                });
                ViewBag.StateList = items;
            }


            return View(globalResult);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GenerateOrder(OrderVM order)
        {
            var role = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value);
            if (role == 1 || role == 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

            var customerId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

            //Insert Header
            var header = new mSalesOrderHeader();
            header.CustomerId = customerId;
            header.ShipToAddressId = order.Address.AddressId;
            header.ShipToMethodId = 1;
            header.OrderDate = DateTime.Now;
            header.DueDate = null;
            header.ShipDate = null;
            header.Status = 1;
            header.CreditCardType = "CREDIT";
            header.CardNumber = order.OrderHeader.CardNumber;
            header.ExpMonth = order.OrderHeader.ExpMonth;
            header.ExpYear = order.OrderHeader.ExpYear;
            header.CreditCardApprovalCode = null;
            header.TotalDue = Convert.ToDecimal(order.OrderHeader.TotalDue);
            header.TaxAmt = Convert.ToDecimal(header.TotalDue) * Convert.ToDecimal(0.12);
            header.SubTotal = header.TotalDue - header.TaxAmt;
            header.Comment = $"1. Estimated delivery date: {DateTime.Now.AddDays(7.0)} (comment at: {DateTime.Now})";

            int headerId;
            using (var service = new CrudService<int, mSalesOrderHeader>("SalesOrderHeaders"))
            {
                var headerInserted = await service.Add(header);
                headerId = headerInserted.SalesOrderId;
            }

            //Insert Detail
            using (var service = new CrudService<int, mShoppingCartItem>("ShoppingCartItems"))
            {
                var items = await service.GetAll();
                var itemList = items.Where(x => x.CustomerId == customerId).ToList();

                //Convertir cada item en line
                for (int i = 0; i < itemList.Count; i++)
                {
                    //lleno con info disponible
                    var line = new mSalesOrderDetail()
                    {
                        SalesOrderId = headerId,
                        ProductId = itemList.ElementAt(i).ProductId,
                        OrderQty = itemList.ElementAt(i).OrderQty
                    };

                    //lleno con info del producto consultado
                    var product = new mProduct();
                    using (var service2 = new CrudService<int, mProduct>("Products"))
                    {
                        var productTemp = await service2.GetOne(itemList.ElementAt(i).ProductId);
                        line.UnitPrice = productTemp.ListPrice;
                        line.UnitPriceDiscount = 0;
                        line.LineTotal = productTemp.ListPrice * line.OrderQty;
                    }

                    //guardo en bd la linea
                    using (var service2 = new CrudService<int, mSalesOrderDetail>("SalesOrderDetails"))
                    {
                        await service2.Add(line);
                    }

                    //elimino el registro del carrito
                    using (var service2 = new CrudService<int, mShoppingCartItem>("ShoppingCartItems"))
                    {
                        await service2.Delete(itemList.ElementAt(i).ShoppingCartItemId);
                    }
                }
                TempData["success"] = "Order created succesfully!";
                return RedirectToAction("Principal", "Home", new { area = "Customer" });
            }
        }


        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var role = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value);
            if (role == 1 || role == 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

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