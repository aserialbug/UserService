using UserService.Domain.Common;

namespace UserService.Domain.User;

public class Password : ValueObject
{
    private const char Divider = ':';

    public Password(PasswordHash hash, Salt salt)
    {
        Hash = hash;
        Salt = salt;
    }

    public PasswordHash Hash { get; }
    public Salt Salt { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Hash;
        yield return Salt;
    }

    public override string ToString()
    {
        return string.Join(Divider, Salt, Hash);
    }

    public static Password Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        var sectors = value.Split(Divider);
        if (sectors.Length != 2)
            throw new ArgumentException($"Value {value} is not valid");

        return new Password(
            PasswordHash.Parse(sectors[1]),
            Salt.Parse(sectors[0]));
    }

    public static bool operator ==(Password? left, Password? right)
    {
        return EqualOperator(left, right);
    }

    public static bool operator !=(Password? left, Password? right)
    {
        return NotEqualOperator(left, right);
    }
}