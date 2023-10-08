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
        
        if (requestContext.Session == null)
            throw new InvalidOperationException("No active session for current request");

        logger.LogInformation(
            "Creating business transaction for request {Request}, trace={Tracing}",
            context.ActionDescriptor.DisplayName,
            requestContext.Session.Id);

        var transaction = await requestContext.Session.StartTransaction();

        var result = await next();
        if (result.Exception == null)
        {
            logger.LogInformation(
                "Commiting business transaction for request {Request}, trace={Tracing}",
                context.ActionDescriptor.DisplayName,
                requestContext.Session.Id);
            try
            {
                await transaction.Commit();
            }
            catch (Exception e)
            {
                logger.LogError(e,
                    "Error committing transaction for request {Request}, rolling back, trace={Tracing}",
                    context.ActionDescriptor.DisplayName,
                    requestContext.Session.Id);
                await transaction.Rollback();
            }
        }
        else
        {
            logger.LogError(result.Exception,
                "Error occurred, transaction for request {Request} will not be committed, trace={Tracing}",
                context.ActionDescriptor.DisplayName,
                requestContext.Session.Id);
            await transaction.Cancel();
        }
    }
}