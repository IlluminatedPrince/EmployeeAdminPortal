using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAdminPortal.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IErrorLogger _errorLogger;

        public ValidationFilter(IErrorLogger errorLogger)
        {
            _errorLogger = errorLogger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .SelectMany(x => x.Value.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                    .ToList();

                var exception = new Exception("Model validation failed: " + string.Join(" | ", errors));
                await _errorLogger.LogAsync(exception);

                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            await next();
        }
    }
}
