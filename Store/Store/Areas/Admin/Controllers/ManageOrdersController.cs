using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using StoreModels.Models;
using System.Security.Claims;

namespace StoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ManageOrdersController : Controller
    {
        private string _apiControllerName = "SalesOrderHeaders";
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManageOrdersController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 1)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            IEnumerable<mSalesOrderHeader> orders;
            using (var service = new CrudService<int, mSalesOrderHeader>(_apiControllerName))
            {
                var result = await service.GetAll();
                orders = result;
            }

            //Extract complementary data
            //customer
            using (var service = new CrudService<int, mCustomer>("Customers"))
            {
                var customer = await service.GetAll();
                ViewBag.Customers = customer.ToDictionary(keySelector: m => m.CustomerId, elementSelector: m => $"{m.FirstName} {m.LastName}");
            }

            //address
            using (var service = new CrudService<int, mAddress>("Addresses"))
            {
                var addresses = await service.GetAll();
                ViewBag.Addresses = addresses.ToDictionary(keySelector: m => m.AddressId, elementSelector: m => $"{m.AddressLine1}");
            }

            return View(orders);
        }

        // GET: ProductController/Details/5
        public async Task<IActionResult> Detail(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 1)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            using (var service = new CrudService<int, mSalesOrderDetail>("SalesOrderDetails"))
            {
                var result = await service.GetAll();
                return View(result.Where(x => x.SalesOrderId == id).ToList());
            }
        }

    }
}
