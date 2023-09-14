using System;
using System.Linq;

namespace UserService.Domain.Base;

public abstract class BaseString : IEquatable<BaseString>
{
    private readonly byte[] _bytes;

    protected BaseString(byte[] bytes)
    {
        _bytes = bytes
            ?? throw new ArgumentNullException(nameof(bytes));
    }
    
    public ReadOnlySpan<byte> GetBytes()
        => new (_bytes);
    
    public override string ToString()
        => Convert.ToBase64String(_bytes);

    public bool Equals(BaseString? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _bytes.SequenceEqual(other._bytes);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BaseString)obj);
    }

    public override int GetHashCode()
    {
        return _bytes
            .Select(b => (int)b)
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(BaseString? left, BaseString? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(BaseString? left, BaseString? right)
    {
        return !Equals(left, right);
    }
}