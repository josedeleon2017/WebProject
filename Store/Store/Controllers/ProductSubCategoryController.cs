using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    public class ProductSubCategoryController : Controller
    {
        // GET: ProductSubCategory
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProductSubCategory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductSubCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductSubCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ProductSubCategory/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductSubCategory/Edit/5
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

        // GET: ProductSubCategory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductSubCategory/Delete/5
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
