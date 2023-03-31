using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using StoreAPI.Models;

namespace Store.Controllers
{
    public class ProductCategoryController : Controller
    {
        private string _apiControllerName = "ProductCategories";

        /// <summary>
        /// Obtains all the categories from the API
        /// </summary>
        /// <returns>A view with all the categories</returns>
        public async Task<IActionResult> Index()
        {
            using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
            {
                return View(await service.GetAll());
            }
        }

        /// <summary>
        /// Obtains one category from the API
        /// </summary>
        /// <param name="id">primary key of dbo.ProductCategory</param>
        /// <returns>A view with one specific category</returns>
        public async Task<IActionResult> Details(int id)
        {
            using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
            {
                return View(await service.GetOne(id));
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Insert one new category object
        /// </summary>
        /// <param name="category">object category to insert</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(mProductCategory category)
        {
            if (ModelState.IsValid)
            {
                using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
                {
                    await service.Add(category);
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

        /// <summary>
        /// Update a category object
        /// </summary>
        /// <param name="id">primary key of dbo.ProductCategory</param>
        /// <param name="category">object category to update</param>
        /// <returns></returns>
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
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
            {
                return View(await service.GetOne(id));
            }
        }

        /// <summary>
        /// Delete a category object
        /// </summary>
        /// <param name="id">primary key of dbo.ProductCategory</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmDelete(int id)
        {
            try
            {
                using (var service = new CrudService<int, mProductCategory>(_apiControllerName))
                {
                    await service.Delete(id);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
