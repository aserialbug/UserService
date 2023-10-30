namespace UserService.Domain.Common;

public abstract class BaseId : ValueObject
{
    public static bool operator ==(BaseId? left, BaseId? right)
    {
        return EqualOperator(left, right);
    }

    public static bool operator !=(BaseId? left, BaseId? right)
    {
        return NotEqualOperator(left, right);
    }
}