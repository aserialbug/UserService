namespace UserService.Application.Models;

public class RequestContext
{
    public TracingId TracingId { get; }


    public RequestContext()
    {
        TracingId = TracingId.New();
    }
}