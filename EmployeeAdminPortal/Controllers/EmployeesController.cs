using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/employees
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees.ToList();
            return Ok(allEmployees);
        }

        // GET: api/employees/{id}
        [HttpGet("{id:guid}")]
        public IActionResult GetEmployee(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // POST: api/employees
        [HttpPost]
        public IActionResult AddEmployee([FromBody] AddEmployeeDto addEmployeeDto)
        {
            var employeeEntity = new Employee
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeDto.Name,
                email = addEmployeeDto.email,
                phone = addEmployeeDto.phone,
                salary = addEmployeeDto.salary
            };

            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetEmployee), new { id = employeeEntity.Id }, employeeEntity);
        }

        // PUT: api/employees/{id}
        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null)
                return NotFound();

            employee.Name = updateEmployeeDto.Name;
            employee.email = updateEmployeeDto.email;
            employee.phone = updateEmployeeDto.phone;
            employee.salary = updateEmployeeDto.salary;

            dbContext.SaveChanges();
            return Ok(employee);
        }
        // DELETE: api/employees/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee == null)
                return NotFound();

            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();

            return Ok(employee);
        }

    }
}
