using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Npgsql;
using UserService.Application.Exceptions;

namespace UserService.Filters;

public class ErrorHandlingFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var executed = await next();
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