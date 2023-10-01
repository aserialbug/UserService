namespace UserService.Application.Models;

public class RequestContext
{
    public RequestContext()
    {
        TracingId = TracingId.New();
    }

    public TracingId TracingId { get; }
}