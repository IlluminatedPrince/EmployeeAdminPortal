using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.Entities
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }  // Primary key

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }
    }
}
