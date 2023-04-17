using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.APIServices;
using StoreModels.Models;

namespace StoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductSubCategoryController : Controller
    {
        private string _apiControllerName = "ProductSubCategories";

        public async Task<IActionResult> Index()
        {
            using (var service = new CrudService<int, mProductCategory>("ProductCategories"))
            {
                var categories = await service.GetAll();
                ViewBag.Categories = categories.ToDictionary(keySelector: m => m.ProductCategoryId, elementSelector: m => m.Name);
            }
            using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
            {
                return View(await service.GetAll());
            }
        }

        public async Task<IActionResult> Create()
        {
            using (var service = new CrudService<int, mProductCategory>("ProductCategories"))
            {
                var categories = await service.GetAll();
                IEnumerable<SelectListItem> items = categories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ProductCategoryId.ToString()
                });
                ViewBag.CategoriesList = items;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(mProductSubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
                {
                    await service.Add(subcategory);
                    TempData["success"] = "Category created succesfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subcategory);
        }

        public async Task<IActionResult> Edit(int id)
        {
            using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
            {
                var subcategory = await service.GetOne(id);
                using (var serviceSec = new CrudService<int, mProductCategory>("ProductCategories"))
                {
                    var categories = await serviceSec.GetAll();
                    IEnumerable<SelectListItem> items = categories.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.ProductCategoryId.ToString(),
                        Selected = subcategory.ProductCategoryId == x.ProductCategoryId
                    });
                    ViewBag.CategoriesList = items;
                }
                return View(subcategory);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, mProductSubCategory subcategory)
        {
            if (id != subcategory.ProductSubCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
                {
                    await service.Update(id, subcategory);
                    TempData["success"] = "Category updated succesfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subcategory);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
            {
                await service.Delete(id);
                TempData["success"] = "SubCategory deleted succesfully!";
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true });
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetSubCategories(string CategoryId)
        {
            using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
            {
                var subCategoriesFilter = await service.GetAll();
                return Json(new SelectList(subCategoriesFilter.Where(x => x.ProductCategoryId.ToString() == CategoryId), "ProductSubCategoryId", "Name"));
            }
        }
    }
}
