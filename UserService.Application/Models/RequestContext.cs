namespace UserService.Application.Models;

public class RequestContext
{
    public SessionId? SessionId { get; private set; }

    public void SetSession(SessionId? session = null)
    {
        SessionId = session ?? SessionId.New();
    }
}