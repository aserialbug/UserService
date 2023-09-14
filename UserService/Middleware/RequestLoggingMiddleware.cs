using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UserService.Application.Models;

namespace UserService.Middleware;

public class RequestLoggingMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestContext _context;

    public RequestLoggingMiddleware(ILoggerFactory loggerFactory, RequestContext context)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("Handling {Method} {Url}; trace={Tracing}",
            context.Request.Method, 
            context.Request.Path.Value,
            _context.TracingId);
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error handling {Method} {Url}: {Message}; trace={Tracing}", 
                context.Request.Method, 
                context.Request.Path.Value,
                e.Message,
                _context.TracingId);
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation("Successfully handled {Method} {Url} => {Status} in {Time} ms; trace={Tracing}", 
                context.Request.Method, 
                context.Request.Path.Value, 
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                _context.TracingId);
        }
    }
}