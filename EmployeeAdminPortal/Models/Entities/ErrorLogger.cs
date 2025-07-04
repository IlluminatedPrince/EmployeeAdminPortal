using EmployeeAdminPortal.Data;

namespace EmployeeAdminPortal.Models.Entities
{
    public interface IErrorLogger
    {
        Task LogAsync(Exception ex);
    }

    public class SqlErrorLogger : IErrorLogger
    {
        private readonly ApplicationDbContext _context;

        public SqlErrorLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(Exception ex)
        {
            var error = new ErrorLog
            {
                Id = Guid.NewGuid(), // ✅ Add this
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                InnerException = ex.InnerException?.Message,
                CreatedAt = DateTime.UtcNow // ✅ Add this
            };

            _context.ErrorLogs.Add(error);
            await _context.SaveChangesAsync();
        }
    }
}
