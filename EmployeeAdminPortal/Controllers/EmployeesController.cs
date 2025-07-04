using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
                return NotFound();
            return Ok(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeDto dto)
        {
            var emp = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Salary = dto.Salary
            };
            _context.Employees.Add(emp);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = emp.EmployeeID }, emp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AddEmployeeDto dto)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
                return NotFound();

            emp.Name = dto.Name;
            emp.Email = dto.Email;
            emp.Phone = dto.Phone;
            emp.Salary = dto.Salary;

            await _context.SaveChangesAsync();
            return Ok(emp);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null)
                return NotFound();

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
