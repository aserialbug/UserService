using UserService.Application.Models;
using UserService.Utils;

namespace UserService.Middleware;

public class SessionMiddleware : IMiddleware
{
    private readonly RequestContext _requestContext;

    public SessionMiddleware(RequestContext requestContext)
    {
        _requestContext = requestContext;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _requestContext.SetSession(context.TryGetSessionId());
        await next(context);
    }
}