using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using StoreAPI.Models;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        private string _apiControllerName = "Products";
        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            using (var service = new CrudService<int, mProduct>(_apiControllerName))
            {
                return View(/*await service.GetAll()*/);
            }
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(mProduct product)
        {
            if (ModelState.IsValid)
            {
                using (var service = new CrudService<int, mProduct>(_apiControllerName))
                {
                    await service.Add(product);
                    TempData["success"] = "Product created succesfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
