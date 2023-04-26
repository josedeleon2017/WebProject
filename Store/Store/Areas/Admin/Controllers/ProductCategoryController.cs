using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using StoreModels.Models;
using System.Security.Claims;

namespace StoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductCategoryController : Controller
    {
        private string _apiControllerName = "ProductCategories";
        public async Task<IActionResult> Index()
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
            {
                return View(await service.GetAll());
            }
        }

        public ActionResult Create()
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(mProductCategory category)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            if (ModelState.IsValid)
            {
                using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
                {
                    await service.Add(category);
                    TempData["success"] = "Category created succesfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
            {
                return View(await service.GetOne(id));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, mProductCategory category)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            if (id != category.ProductCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
                {
                    await service.Update(id, category);
                    TempData["success"] = "Category updated succesfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
            {
                TempData["success"] = "Category deleted succesfully!";
                await service.Delete(id);
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true });
            }
        }

    }
}
