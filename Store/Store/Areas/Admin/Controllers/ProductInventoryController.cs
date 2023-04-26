using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.APIServices;
using StoreModels.Models;
using System.Security.Claims;

namespace StoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductInventoryController : Controller
    {
        private string _apiControllerName = "ProductInventories";


        public async Task<IActionResult> Create(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            using (var service = new CrudService<int, mProduct>("Products"))
            {
                var product = await service.GetOne(id);
                ViewBag.Product = product;
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(mProductInventory productInventory)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            if (ModelState.IsValid)
            {
                using (var service = new CrudService<int, mProductInventory>(_apiControllerName))
                {
                    await service.Add(productInventory);
                    TempData["success"] = "ProductInventory registered succesfully!";
                }

                //Actualiza los campos del inventario en Product
                using (var service = new CrudService<int, mProduct>("Products"))
                {
                    var productUpdate = await service.GetOne(productInventory.ProductId);
                    productUpdate.ActiveFlag = productInventory.Quantity > 0;
                    productUpdate.LowStock = productInventory.Quantity <= productInventory.SafetyStockLevel;
                    await service.Update(productInventory.ProductId, productUpdate);
                }
                return RedirectToAction("Index", "Product");
            }
            return View(productInventory);
        }


        // GET: ProductInventoryController/Details/5
        public async Task<IActionResult> Inventory(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            using (var service = new CrudService<int, mProductInventory>(_apiControllerName))
            {
                var inventory = await service.GetOne(id);
                if (inventory == null)
                {
                    return RedirectToAction("Create", new { id });
                }
                using (var serviceSec = new CrudService<int, mProduct>("Products"))
                {
                    var product = await serviceSec.GetOne(id);
                    ViewBag.Product = product;
                }
                return View(inventory);
            }
        }



        // GET: ProductInventoryController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            using (var service = new CrudService<int, mProduct>("Products"))
            {
                var product = await service.GetOne(id);
                ViewBag.Product = product;
            }
            using (var service = new CrudService<int, mProductInventory>(_apiControllerName))
            {
                return View(await service.GetOne(id));
            }
        }

        // POST: ProductInventoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, mProductInventory productInventory)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            if (id != productInventory.ProductId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //Actualiza el registro de Inventory
                using (var service = new CrudService<int, mProductInventory>(_apiControllerName))
                {
                    await service.Update(id, productInventory);
                    TempData["success"] = "ProductInventory updated succesfully!";
                }

                //Actualiza los campos del inventario en Product
                using (var service = new CrudService<int, mProduct>("Products"))
                {
                    //ActiveFlag se actualizará automáticamente Quantity <= 0
                    //LowStock dependerá sí en la tabla dbo.ProductInventory, SafetyStockLevel <= Quantity
                    var productUpdate = await service.GetOne(productInventory.ProductId);
                    productUpdate.ActiveFlag = productInventory.Quantity > 0;
                    productUpdate.LowStock = productInventory.Quantity <= productInventory.SafetyStockLevel;
                    await service.Update(productInventory.ProductId, productUpdate);
                }
                return RedirectToAction("Inventory", new { id });
            }
            return View(productInventory);
        }


    }
}
