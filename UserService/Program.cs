using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using UserService.Application;
using UserService.Filters;
using UserService.Infrastructure;
using UserService.Infrastructure.Services;
using UserService.Middleware;
using UserService.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddTransient<ErrorHandlingFilter>();
builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilter>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>("Bearer");
});
builder.Services.AddScoped<RequestLoggingMiddleware>();
builder.Services.Configure<KestrelServerOptions>(
    builder.Configuration.GetSection("Kestrel"));
builder.Logging.AddConsole();


var app = builder.Build();
app.Logger.LogInformation("Applying database migrations...");
try
{
    var migrations = app.Services.GetRequiredService<MigrationsService>();
    await migrations.Apply();
}
catch (Exception e)
{
    app.Logger.LogCritical(e,"Database migrations error: {Message}, exiting", e.Message);
    return;
}

app.Logger.LogInformation("Migrations applied successfully");


// Configure the HTTP request pipeline.
app.UseMiddleware<RequestLoggingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
