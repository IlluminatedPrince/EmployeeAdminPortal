namespace EmployeeAdminPortal.Models
{
    public class UpdateEmployeeDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string email { get; set; }
        public string? phone { get; set; }
        public decimal salary { get; set; }
    }
}
