using MassTransit;

namespace UserService.Application.Models;

public sealed class SessionId : IEquatable<SessionId>
{
    private readonly NewId _id;

    private SessionId(NewId id)
    {
        _id = id;
    }

    public bool Equals(SessionId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _id == other._id;
    }

    public override string ToString()
        => $"{nameof(SessionId)}_{_id:N}";

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is SessionId other && Equals(other));
    }

    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }

    public static bool operator ==(SessionId? left, SessionId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(SessionId? left, SessionId? right)
    {
        return !Equals(left, right);
    }
    
    public static SessionId Parse(string value)
    {
        var idTmp = value.Split('_');

        if (idTmp.Length != 2 ||
            !idTmp[0].Equals(nameof(SessionId)))
            throw new ArgumentException($"Value {value} is invalid {nameof(SessionId)}");
        
        var id = new NewId(idTmp[1]);
        
        return new SessionId(id);
    }

    public static SessionId New()
    {
        return new(NewId.Next());
    }
}