using Microsoft.EntityFrameworkCore;
using EmployeeAdminPortal.Models.Entities; // Correct reference to your Employee class

namespace EmployeeAdminPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)

        {
        }

        public DbSet<ApiKey> ApiKeys { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}
