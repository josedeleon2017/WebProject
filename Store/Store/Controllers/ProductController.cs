using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.APIServices;
using StoreModels.Models;
using System.Net.WebSockets;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        private string _apiControllerName = "Products";
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            using (var service = new CrudService<int, mProduct>(_apiControllerName))
            {
                return View(await service.GetAll());
            }
        }

        // GET: ProductController/Details/5
        public async Task<IActionResult> Preview(int id)
        {
            using (var service = new CrudService<int, mProduct>(_apiControllerName))
            {
                return View(await service.GetOne(id));
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
        public async Task<IActionResult> Create(mProduct product, IFormFile? file)
        {
            ModelState.Remove("ImagePath");
            if (ModelState.IsValid)
            {
                string wwwPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwPath, @"storage\products");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    product.ImagePath = @"\storage\products\" + fileName;
                }
                else
                {
                    product.ImagePath = @"\storage\products\product_default.jpg";
                }

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
        public async Task<IActionResult> Edit(int id)
        {
            //Obtiene el product
            mProduct product;
            using (var service = new CrudService<int, mProduct>(_apiControllerName))
            {
                product = await service.GetOne(id);
            }
            //Obtiene el Id del padre
            int categoryId;
            using (var service = new CrudService<int, mProductSubCategory>("ProductSubCategories"))
            {
                var result = await service.GetOne(product.ProductSubCategoryId);
                categoryId = result.ProductCategoryId;
            }
            //Obtiene todos los padres y selecciona el especifico
            using (var service = new CrudService<int, mProductCategory>("ProductCategories"))
            {
                var categories = await service.GetAll();
                IEnumerable<SelectListItem> items = categories.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ProductCategoryId.ToString(),
                    Selected = x.ProductCategoryId == categoryId
                });
                ViewBag.CategoriesList = items;
            }
            //Obtiene todos los hijos con base al padre
            using (var service = new CrudService<int, mProductSubCategory>("ProductSubCategories"))
            {
                var subcategories = await service.GetAll();
                var subFilter = subcategories.Where(x => x.ProductCategoryId == categoryId);
                IEnumerable<SelectListItem> items = subFilter.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ProductSubCategoryId.ToString(),
                    Selected = x.ProductSubCategoryId == product.ProductSubCategoryId
                });
                ViewBag.SubCategoriesList = items;
            }
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, mProduct product, IFormFile? file)
        {

            if (id != product.ProductId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string wwwPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwPath, @"storage\products");

                    //delete old image
                    if (!string.IsNullOrEmpty(product.ImagePath))
                    {
                        var pathImageDelete = Path.Combine(wwwPath, product.ImagePath.TrimStart('\\'));
                        if (System.IO.File.Exists(pathImageDelete))
                        {
                            System.IO.File.Delete(pathImageDelete);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    product.ImagePath = @"\storage\products\" + fileName;
                }

                using (var service = new CrudService<int, mProduct>(_apiControllerName))
                {
                    await service.Update(id, product);
                    TempData["success"] = "Product updated succesfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: ProductController/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            //delete image
            using (var service = new CrudService<int, mProduct>(_apiControllerName))
            {
                var productToDelete = await service.GetOne(id);
                if (!string.IsNullOrEmpty(productToDelete.ImagePath))
                {
                    string wwwPath = _webHostEnvironment.WebRootPath;
                    var pathImageDelete = Path.Combine(wwwPath, productToDelete.ImagePath.TrimStart('\\'));
                    if (System.IO.File.Exists(pathImageDelete))
                    {
                        System.IO.File.Delete(pathImageDelete);
                    }
                }
            }

            //delete inventory
            using (var service = new CrudService<int, mProductInventory>("ProductInventories"))
            {
                await service.Delete(id);
            }

            //delete object
            using (var service = new CrudService<int, mProduct>(_apiControllerName))
            {
                TempData["success"] = "Product deleted succesfully!";
                await service.Delete(id);
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true });
            }
        }

        //#region API
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    using (var service = new CrudService<int, mProduct>(_apiControllerName))
        //    {
        //        var products = await service.GetAll();
        //        return Json(new { data = products });
        //    }
        //}


        //#endregion

    }
}
