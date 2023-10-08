using UserService.Application.Models;
using UserService.Utils;

namespace UserService.Middleware;

public class SessionMiddleware : IMiddleware
{
    private readonly SessionManager _sessionManager;
    private readonly RequestContext _requestContext;

    public SessionMiddleware(SessionManager sessionManager, RequestContext requestContext)
    {
        _sessionManager = sessionManager;
        _requestContext = requestContext;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var session = await _sessionManager.StartSession(context.TryGetSessionId());

        try
        {
            _requestContext.SetSession(session);
            await next(context);
        }
        finally
        {
            await session.Free();
        }
    }
}