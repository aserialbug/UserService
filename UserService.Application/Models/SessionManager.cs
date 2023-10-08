using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace UserService.Application.Models;

public class SessionManager
{
    private readonly ILogger<SessionManager> _logger;
    private readonly ConcurrentDictionary<SessionId, Session> _sessions = new();

    public SessionManager(ILogger<SessionManager> logger)
    {
        _logger = logger;
    }

    public Task<Session> StartSession(SessionId? sessionId = default)
    {
        var session = _sessions.GetOrAdd(sessionId ?? SessionId.New(), id => new Session(id));
        session.Completed += SessionOnCompleted;
        
        _logger.LogInformation("New session {Id} started", session.Id);
        return Task.FromResult(session);
    }

    private void SessionOnCompleted(object? sender, SessionCompletedEventArgs e)
    {
        _sessions.Remove(e.SessionId, out _);
        _logger.LogInformation("Session {Id} completed", e.SessionId);
    }
}