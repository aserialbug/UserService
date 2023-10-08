namespace UserService.Application.Models;

public class RequestContext
{
    public Session? Session { get; private set; }

    public void SetSession(Session session)
    {
        if (Session != null)
            throw new InvalidOperationException("Session context has already been initialized!");

        Session = session;
    }
}