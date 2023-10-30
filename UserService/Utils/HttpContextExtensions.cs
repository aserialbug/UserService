using UserService.Application.Models;

namespace UserService.Utils;

public static class HttpContextExtensions
{
    private const string SessionHeaderName = "x-custom-session-id";
    public static SessionId? TryGetSessionId(this HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue(SessionHeaderName, out var headerValue))
            return null;

        return SessionId.Parse(headerValue.First());
    }
}