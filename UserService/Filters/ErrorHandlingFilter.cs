﻿using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Npgsql;
using UserService.Application.Exceptions;

namespace UserService.Filters;

public class ErrorHandlingFilter : IAsyncActionFilter
{
    private readonly ILogger _logger;

    public ErrorHandlingFilter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ErrorHandlingFilter>();
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executed = await next();
        if (executed.Exception == null)
        {
            return;
        }
        
        _logger.LogError(executed.Exception, "Error occurred: {Message}", executed.Exception.Message);
        
        switch (executed.Exception)
        {
            case ArgumentException:
                executed.Result = new ObjectResult(executed.Exception.Message)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                executed.ExceptionHandled = true;
                break;
            case NotFoundException:
                executed.Result = new ObjectResult(executed.Exception.Message)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
                executed.ExceptionHandled = true;
                break;
            case SocketException or PostgresException:
                executed.Result = new ObjectResult(executed.Exception.Message)
                {
                    StatusCode = StatusCodes.Status503ServiceUnavailable
                };
                executed.ExceptionHandled = true;
                break;
            case not null:
                executed.Result = new ObjectResult($"Unknown error: {executed.Exception.Message}")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                executed.ExceptionHandled = true;
                break;
        }
    }
}