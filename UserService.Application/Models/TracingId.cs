namespace UserService.Application.Models;

public sealed class TracingId : IEquatable<TracingId>
{
    private readonly string _tracingId;

    private TracingId(string tracingId)
    {
        _tracingId = tracingId;
    }

    public bool Equals(TracingId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _tracingId == other._tracingId;
    }

    public override string ToString()
    {
        return _tracingId;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is TracingId other && Equals(other));
    }

    public override int GetHashCode()
    {
        return _tracingId.GetHashCode();
    }

    public static bool operator ==(TracingId? left, TracingId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TracingId? left, TracingId? right)
    {
        return !Equals(left, right);
    }

    public static TracingId New()
    {
        return new($"TracingId_{Guid.NewGuid():N}");
    }
}