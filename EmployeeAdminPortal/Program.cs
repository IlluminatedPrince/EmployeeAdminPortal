using EmployeeAdminPortal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Admin Portal API", Version = "v1" });
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. Enter in 'x-api-key: YOUR_KEY'",
        In = ParameterLocation.Header,
        Name = "x-api-key",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>()
        }
    });

});



// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("EmployeeAdminPortal.Data"))); // Ensure this matches your project structure

// Add Application Insights
if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
    });
}

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Admin Portal API v1"));
}

else
{
    app.UseHsts(); // Enable HTTP Strict Transport Security in production
}
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Admin Portal API v1"));
app.UseHttpsRedirection();
app.UseRouting();

// Use CORS before authorization
app.UseCors("AllowAll");
app.Use(async (context, next) =>
{
    var key = context.Request.Headers["x-api-key"].FirstOrDefault();

    if (string.IsNullOrEmpty(key))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("API Key is missing.");
        return;
    }

    var db = context.RequestServices.GetRequiredService<ApplicationDbContext>();

    var isValid = await db.ApiKeys.AnyAsync(k => k.KeyValue == key && k.IsActive);

    if (!isValid)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Invalid API Key.");
        return;
    }

    await next();
});

// Authentication and Authorization
// app.UseAuthentication(); // Uncomment if you implement authentication
app.UseAuthorization();

app.MapControllers();

// Redirect root to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription();

app.Run();