using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;
using StoreModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly StoreContext _context;

        public EmployeesController(StoreContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<mEmployee>>> GetEmployees()
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var listResult = new List<mEmployee>();
            await _context.Employees.ForEachAsync(x => listResult.Add(new mEmployee()
            {
                EmployeeId = x.EmployeeId,
                RoleId = x.RoleId,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                LoginId = x.LoginId,
                Password = x.Password,
                NationalNumberId = x.NationalNumberId,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                PhotoPath = x.PhotoPath,
            }));
            return listResult;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<mEmployee>> GetEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            var result = new mEmployee()
            {
                EmployeeId = employee.EmployeeId,
                RoleId = employee.RoleId,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                LoginId = employee.LoginId,
                Password = employee.Password,
                NationalNumberId = employee.NationalNumberId,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                PhotoPath = employee.PhotoPath,
            };
            return result;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, mEmployee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            var employeeBase = _context.Employees.FirstOrDefault(x => x.EmployeeId == id);
            if (employeeBase != null)
            {
                employeeBase.RoleId = employee.RoleId;
                employeeBase.FirstName = employee.FirstName;
                employeeBase.MiddleName = employee.MiddleName;
                employeeBase.LastName = employee.LastName;
                employeeBase.LoginId = employee.LoginId;
                employeeBase.Password = employee.Password;
                employeeBase.NationalNumberId = employee.NationalNumberId;
                employeeBase.BirthDate = employee.BirthDate;
                employeeBase.Gender = employee.Gender;
                employeeBase.PhotoPath = employee.PhotoPath;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<mEmployee>> PostEmployee(mEmployee employee)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'StoreContext.Employees'  is null.");
            }
            var employeeToInsert = new Employee()
            {
                EmployeeId = employee.EmployeeId,
                RoleId = employee.RoleId,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                LoginId = employee.LoginId,
                Password = employee.Password,
                NationalNumberId = employee.NationalNumberId,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                PhotoPath = employee.PhotoPath,
            };
            _context.Employees.Add(employeeToInsert);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
