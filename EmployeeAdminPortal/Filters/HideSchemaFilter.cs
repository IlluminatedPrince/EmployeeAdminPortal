using EmployeeAdminPortal.Models.Entities;
using EmployeeAdminPortal.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class HideSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var hiddenTypes = new[]
        {
            typeof(AddEmployeeDto),
            typeof(UpdateEmployeeDto),
            typeof(Employee)
        };

        if (hiddenTypes.Contains(context.Type))
        {
            // Null out the schema so it doesn't render
            schema.Description = "Hidden schema (not shown in Swagger)";
            schema.Properties.Clear();
        }
    }
}
