using Microsoft.AspNetCore.Mvc.Filters;
using UserService.Application.Models;
using UserService.Infrastructure.Context;

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
        
        if (requestContext.SessionId == null)
            throw new InvalidOperationException("No active session for current request");
        
        var entityContext = context.HttpContext
            .RequestServices
            .GetRequiredService<EntitiesContext>();

        logger.LogInformation(
            "Creating business transaction for request {Request}, trace={Tracing}",
            context.ActionDescriptor.DisplayName,
            requestContext.SessionId);

        await using var transaction = await entityContext.BeginTransactionAsync();
        var result = await next();
        if (result.Exception != null)
        {
            logger.LogError(result.Exception,
                "Error occurred, transaction for request {Request} will not be committed, trace={Tracing}",
                context.ActionDescriptor.DisplayName,
                requestContext.SessionId);
            await transaction.Rollback();
            return;
        }

        try
        {
            await entityContext.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(result.Exception,
                "Error saving changes for request {Request} rolling back transaction, trace={Tracing}",
                context.ActionDescriptor.DisplayName,
                requestContext.SessionId);
            result.Exception = exception;
            await transaction.Rollback();
            return;
        }
        

        await transaction.Commit();
    }
}