using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using StoreAPI.Models;

namespace Store.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: mProductCategoryController
        public async Task<IActionResult> Index()
        {
            return View(await ProductCategoryService.GetProductCategories());
        }

        // GET: mProductCategoryController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View(await ProductCategoryService.GetProductCategory(id));
        }

        // GET: mProductCategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: mProductCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(mProductCategory category)
        {
            if (ModelState.IsValid)
            {
                await ProductCategoryService.AddProductCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: mProductCategoryController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View(await ProductCategoryService.GetProductCategory(id));
        }

        // POST: mProductCategoryController/Edit/5
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
                await ProductCategoryService.UpdateProductCategory(id, category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: mProductCategoryController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return View(await ProductCategoryService.GetProductCategory(id));
        }

        // POST: mProductCategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmDelete(int id)
        {
            try
            {
                await ProductCategoryService.DeleteProductCategory(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
