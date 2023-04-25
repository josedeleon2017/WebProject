using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.APIServices;
using StoreModels.Models;
using StoreMVC.Models;
using System.Security.Claims;

namespace StoreMVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class IdentityController : Controller
    {

        private string _apiControllerName = "Customers";

        // GET: IdentityController/Edit/5
        public ActionResult Login()
        {
            return View();
        }

        // POST: IdentityController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(string email, string password)
        //{
        //    try
        //    {

        //        using (var service = new CrudService<int, mCustomer>(_apiControllerName))
        //        {
        //            var customers = await service.GetAll();
        //            var customerMatch = customers.FirstOrDefault(x => x.EmailAddress == email);
        //            if (customerMatch == null)
        //            {
        //                TempData["error"] = "Customer not found!";
        //                return View();
        //            }
        //            List<Claim> claims = new List<Claim>()
        //            {
        //                //new Claim(ClaimTypes.Name, customerMatch.EmailAddress)
        //            };
        //            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //            AuthenticationProperties properties = new AuthenticationProperties() { AllowRefresh = true };
        //            await HttpContext.SignInAsync(
        //                CookieAuthenticationDefaults.AuthenticationScheme,
        //                new ClaimsPrincipal(claimsIdentity),
        //                properties);
        //        }
        //        return RedirectToAction("Index", "Home");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: IdentityController/Edit/5
        public async Task<IActionResult> Register()
        {
            using (var service = new CrudService<int, mRegion>("Regions"))
            {
                var regions = await service.GetAll();
                IEnumerable<SelectListItem> items = regions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.RegionId.ToString()
                });
                ViewBag.RegionList = items;
            }
            return View();
        }

        // POST: IdentityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerAddressVM ca)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //insert address
                    using (var service = new CrudService<int, mAddress>("Addresses"))
                    {
                        var addressId = await service.Add(ca.Address);
                        ca.Customer.AddressId = addressId.AddressId;
                    }

                    //insert customer
                    ca.Customer.Password = Utilities.Cipher.EncryptPassword(ca.Customer.Password);
                    using (var service = new CrudService<int, mCustomer>(_apiControllerName))
                    {
                        await service.Add(ca.Customer);
                        TempData["success"] = "Customer created succesfully!";
                    }
                    return RedirectToAction("Login", "Identity");
                }
                TempData["error"] = "Customer no created!";
                return View(ca);
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<JsonResult> GetStates(string stateId)
        {
            using (var service = new CrudService<int, mState>("States"))
            {
                var statesFilter = await service.GetAll();
                var selectList = new SelectList(statesFilter.Where(x => x.RegionId.ToString() == stateId), "StateId", "Name");
                return Json(selectList);
            }
        }

    }
}
