namespace UserService.Application.Models;

public class SessionCompletedEventArgs : EventArgs
{
    public SessionId SessionId { get; }
    
    public SessionCompletedEventArgs(SessionId sessionId)
    {
        SessionId = sessionId;
    }
}