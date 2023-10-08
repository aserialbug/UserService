using MassTransit;

namespace UserService.Application.Models;

public class BusinessTransactionId : IEquatable<BusinessTransactionId>
{
    private readonly NewId _id;

    private BusinessTransactionId(NewId id)
    {
        _id = id;
    }
    
    public override string ToString()
        => $"{nameof(BusinessTransactionId)}_{_id:N}";

    public static BusinessTransactionId New()
        => new(NewId.Next());

    public bool Equals(BusinessTransactionId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _id.Equals(other._id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BusinessTransactionId)obj);
    }

    public override int GetHashCode()
    {
        return _id.GetHashCode();
    }

    public static bool operator ==(BusinessTransactionId? left, BusinessTransactionId? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(BusinessTransactionId? left, BusinessTransactionId? right)
    {
        return !Equals(left, right);
    }
}