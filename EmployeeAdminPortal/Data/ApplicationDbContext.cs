using Microsoft.EntityFrameworkCore;
using EmployeeAdminPortal.Models.Entities;

namespace EmployeeAdminPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)

        {
        }

        /*public DbSet<ApiKey> ApiKeys { get; set; }*/
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

    }
}
