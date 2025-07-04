using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using EmployeeAdminPortal.Models.Entities;

namespace EmployeeAdminPortal.Middleware
{
    public class BadRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public BadRequestLoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var originalBody = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            await _next(context); // Continue pipeline

            if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
            {
                newBody.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(newBody).ReadToEndAsync();

                using var scope = _scopeFactory.CreateScope();
                var errorLogger = scope.ServiceProvider.GetRequiredService<IErrorLogger>();

                await errorLogger.LogAsync(new Exception(
                    $"400 Bad Request on path {context.Request.Path}\nResponse: {responseBody}"));
            }

            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
            context.Response.Body = originalBody;
        }
    }
}
