using Microsoft.AspNetCore.Mvc.Filters;
using UserService.Application.Models;

namespace UserService.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class BusinessTransactionAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var logger = context.HttpContext
            .RequestServices
            .GetRequiredService<ILogger<BusinessTransactionAttribute>>();
        
        var requestContext = context.HttpContext
            .RequestServices
            .GetRequiredService<RequestContext>();
        
        logger.LogInformation(
            "Creating business transaction for request {Request}, trace={Tracing}",
            context.ActionDescriptor.DisplayName,
            requestContext.TracingId);

        var result = await next();
        if (result.Exception == null)
        {
            logger.LogInformation(
                "Commiting business transaction for request {Request}, trace={Tracing}",
                context.ActionDescriptor.DisplayName,
                requestContext.TracingId);
        }
        else
        {
            logger.LogInformation(
                "Error occurred, rolling back business transaction for request {Request}, trace={Tracing}",
                context.ActionDescriptor.DisplayName,
                requestContext.TracingId);
        }
    }
}