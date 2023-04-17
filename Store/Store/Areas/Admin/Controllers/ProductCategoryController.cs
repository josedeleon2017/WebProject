using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using StoreModels.Models;

namespace StoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private string _apiControllerName = "ProductCategories";
        public async Task<IActionResult> Index()
        {
            using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
            {
                return View(await service.GetAll());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(mProductCategory category)
        {
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
            using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
            {
                return View(await service.GetOne(id));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, mProductCategory category)
        {
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
