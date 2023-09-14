using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Application;
using UserService.Filters;
using UserService.Infrastructure;
using UserService.Infrastructure.Context;
using UserService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddTransient<ErrorHandlingFilter>();
builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilter>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<RequestLoggingMiddleware>();
builder.Logging.AddConsole();


var app = builder.Build();
app.Logger.LogInformation("Applying database migrations...");
try
{
    var migrations = app.Services.GetRequiredService<Migrations>();
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
app.UseAuthorization();
app.MapControllers();

app.Run();
