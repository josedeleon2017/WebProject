using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using StoreAPI.Models;

namespace Store.Controllers
{
    public class ProductSubCategoryController : Controller
    {
        private string _apiControllerName = "ProductSubCategories";

        /// <summary>
        /// Obtains all the subcategories from the API
        /// </summary>
        /// <returns>A view with all the subcategories</returns>
        public async Task<IActionResult> Index()
        {
            using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
            {
                return View(await service.GetAll());
            }
        }

        /// <summary>
        /// Obtains one subcategory from the API
        /// </summary>
        /// <param name="id">primary key of dbo.ProductSubCategory</param>
        /// <returns>A view with one specific subcategory</returns>
        public async Task<IActionResult> Details(int id)
        {
            using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
            {
                return View(await service.GetOne(id));
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Insert one new subcategory object
        /// </summary>
        /// <param name="category">object subcategory to insert</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(mProductSubCategory subcategory)
        {
            if (ModelState.IsValid)
            {
                using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
                {
                    await service.Add(subcategory);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subcategory);
        }

        public async Task<IActionResult> Edit(int id)
        {
            using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
            {
                return View(await service.GetOne(id));
            }
        }

        /// <summary>
        /// Update a subcategory object
        /// </summary>
        /// <param name="id">primary key of dbo.ProductSubCategory</param>
        /// <param name="category">object subcategory to update</param>
        /// <returns></returns>
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
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subcategory);
        }

        public async Task<IActionResult> Delete(int id)
        {
            using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
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
                using (var service = new CrudService<int, mProductSubCategory>(_apiControllerName))
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
