using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIServices;
using StoreModels.Models;
using System.Security.Claims;
using System.Xml.Linq;

namespace StoreMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EmployeeController : Controller
    {
        private string _apiControllerName = "Employees";
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: EmployeeController
        public async Task<IActionResult> Index()
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

            using (var service = new CrudService<int, mEmployee>(_apiControllerName))
            {
                var list = await service.GetAll();
                for (int i = 0; i < list.Count(); i++)
                {
                    list.ElementAt(i).Password = Utilities.Cipher.Decrypt(list.ElementAt(i).Password);
                }
                return View(list);
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
        public async Task<IActionResult> Create(mEmployee employee, IFormFile? PhotoPath)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

            //ModelState.Remove("PhotoPath");
            ModelState.Remove("LoginId");
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                string wwwPath = _webHostEnvironment.WebRootPath;
                if (PhotoPath != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoPath.FileName);
                    string productPath = Path.Combine(wwwPath, @"storage\employees");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        PhotoPath.CopyTo(fileStream);
                    }
                    employee.PhotoPath = @"\storage\employees\" + fileName;
                }
                else
                {
                    employee.PhotoPath = @"\storage\employees\product_default.jpg";
                }

                using (var service = new CrudService<int, mEmployee>(_apiControllerName))
                {
                    employee.Password = Utilities.Cipher.Encrypt("web2023");
                    using (var serviceAux = new CrudService<int, mEmployee>(_apiControllerName))
                    {
                        var employees = await service.GetAll();
                        var currentID = employee.FirstName.Trim().ToUpper() + employee.LastName.Trim().ToUpper();
                        bool notExist = false;
                        while (!notExist)
                        {
                            var test = employees.Where(x => x.LoginId == currentID).FirstOrDefault();
                            if (test == null)
                            {
                                employee.LoginId = currentID;
                                break;
                            }
                            else
                            {
                                currentID += new Random().Next(1, 9).ToString();
                            }
                        }
                    }
                    await service.Add(employee);
                    TempData["success"] = "Employee created succesfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

            using (var service = new CrudService<int, mEmployee>(_apiControllerName))
            {
                var employee = await service.GetOne(id);
                employee.Password = Utilities.Cipher.Decrypt(employee.Password);
                return View(employee);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, mEmployee employee, IFormFile? PhotoPath)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 1)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string wwwPath = _webHostEnvironment.WebRootPath;
                if (PhotoPath != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoPath.FileName);
                    string productPath = Path.Combine(wwwPath, @"storage\employees");

                    //delete old image
                    if (!string.IsNullOrEmpty(employee.PhotoPath))
                    {
                        var pathImageDelete = Path.Combine(wwwPath, employee.PhotoPath.TrimStart('\\'));
                        if (System.IO.File.Exists(pathImageDelete))
                        {
                            System.IO.File.Delete(pathImageDelete);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        PhotoPath.CopyTo(fileStream);
                    }
                    employee.PhotoPath = @"\storage\employees\" + fileName;
                }

                using (var service = new CrudService<int, mEmployee>(_apiControllerName))
                {
                    employee.Password = Utilities.Cipher.Encrypt(employee.Password);
                    await service.Update(id, employee);
                    TempData["success"] = "Employee updated succesfully!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> EditLimited(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 1)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

            using (var service = new CrudService<int, mEmployee>(_apiControllerName))
            {
                var employee = await service.GetOne(id);
                employee.Password = Utilities.Cipher.Decrypt(employee.Password);
                return View(employee);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLimited(int id, mEmployee employee, IFormFile? PhotoPath)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 1)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string wwwPath = _webHostEnvironment.WebRootPath;
                if (PhotoPath != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoPath.FileName);
                    string productPath = Path.Combine(wwwPath, @"storage\employees");

                    //delete old image
                    if (!string.IsNullOrEmpty(employee.PhotoPath))
                    {
                        var pathImageDelete = Path.Combine(wwwPath, employee.PhotoPath.TrimStart('\\'));
                        if (System.IO.File.Exists(pathImageDelete))
                        {
                            System.IO.File.Delete(pathImageDelete);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        PhotoPath.CopyTo(fileStream);
                    }
                    employee.PhotoPath = @"\storage\employees\" + fileName;
                }

                using (var service = new CrudService<int, mEmployee>(_apiControllerName))
                {
                    employee.Password = Utilities.Cipher.Encrypt(employee.Password);
                    await service.Update(id, employee);
                    TempData["success"] = "Employee updated succesfully!";
                }
                return RedirectToAction("pEmployee", "Employee", new { area = "Admin" });
            }
            return View(employee);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }

            //delete image
            using (var service = new CrudService<int, mEmployee>(_apiControllerName))
            {
                var employeeToDelete = await service.GetOne(id);
                if (!string.IsNullOrEmpty(employeeToDelete.PhotoPath))
                {
                    string wwwPath = _webHostEnvironment.WebRootPath;
                    var pathImageDelete = Path.Combine(wwwPath, employeeToDelete.PhotoPath.TrimStart('\\'));
                    if (System.IO.File.Exists(pathImageDelete))
                    {
                        System.IO.File.Delete(pathImageDelete);
                    }
                }
            }

            //delete field
            using (var service = new CrudService<int, mEmployee>(_apiControllerName))
            {
                TempData["success"] = "Employee deleted succesfully!";
                await service.Delete(id);
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true });
            }
        }


        public async Task<IActionResult> pEmployee()
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 1)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            using (var service = new CrudService<int, mEmployee>("Employees"))
            {
                var id = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                return View(await service.GetOne(id));
            }
        }

        public async Task<IActionResult> pAdmin()
        {
            if (Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value) != 2)
            {
                return RedirectToAction("Logout", "Identity", new { area = "Identity" });
            }
            using (var service = new CrudService<int, mEmployee>("Employees"))
            {
                var id = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                return View(await service.GetOne(id));
            }
        }

    }
}
