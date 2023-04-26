using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.APIServices;
using StoreModels.Models;
using StoreMVC.Areas.Customer.Controllers;
using StoreMVC.Models;
using System;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace StoreMVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class IdentityController : Controller
    {

        //private string _apiControllerName = "Customers";

        // GET: IdentityController/Edit/5
        public ActionResult Login(string? returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        //POST: IdentityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login)
        {
            try
            {
                using (var service = new CrudService<int, mCustomer>("Customers"))
                {
                    //busca un usuario con ese correo
                    var customers = await service.GetAll();
                    var customerMatch = customers.FirstOrDefault(x => x.EmailAddress == login.Email);
                    if (customerMatch == null)
                    {
                        TempData["error"] = "User doesnt exist!";
                        return View(login);
                    }

                    //valida que las credenciales concuerden
                    if (customerMatch.EmailAddress == login.Email && customerMatch.Password == Utilities.Cipher.Encrypt(login.Password))
                    {
                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(customerMatch.CustomerId)),
                            new Claim(ClaimTypes.Email, customerMatch.EmailAddress),
                            new Claim("userName", $"{customerMatch.FirstName.ToUpper()} {customerMatch.LastName.ToUpper()}")
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);

                        if (Url.IsLocalUrl(TempData["returnUrl"].ToString()))
                        {
                            return Redirect(TempData["returnUrl"].ToString());
                        }
                        else
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                    else
                    {
                        TempData["error"] = "Credentials not match!";
                        return View(login);
                    }
                    //AuthenticationProperties properties = new AuthenticationProperties() { AllowRefresh = true };
                    //await HttpContext.SignInAsync(
                    //    CookieAuthenticationDefaults.AuthenticationScheme,
                    //    new ClaimsPrincipal(claimsIdentity),
                    //    properties);
                }
                //return View(login);
            }
            catch
            {
                return View();
            }
        }


        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Principal", "Home", new { area = "Customer" });

        }

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
            {   //valida que no exista un cliente con el mismo correo                
                using (var service = new CrudService<int, mCustomer>("Customers"))
                {
                    var customer = await service.GetAll();
                    if (customer.FirstOrDefault(x => x.EmailAddress == ca.Customer.EmailAddress) != null)
                    {
                        TempData["error"] = "User alredy exist!";
                        return View(ca);
                    }
                }
                //si los datos son validos se almacena el registro
                if (ModelState.IsValid)
                {
                    //insert address
                    using (var service = new CrudService<int, mAddress>("Addresses"))
                    {
                        var addressObj = await service.Add(ca.Address);
                        ca.Customer.AddressId = addressObj.AddressId;
                    }

                    //insert customer
                    ca.Customer.Password = Utilities.Cipher.Encrypt(ca.Customer.Password);
                    using (var service = new CrudService<int, mCustomer>("Customers"))
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
                TempData["error"] = "Customer no created! (exception)";
                return View(ca);
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
