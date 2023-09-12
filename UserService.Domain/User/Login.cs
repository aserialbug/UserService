using UserService.Domain.Base;

namespace UserService.Domain.User;

public class Login : ValueObject
{
    private readonly string _login;

    private Login(string login)
    {
        _login = login;
    }

    public static Login Parse(string? value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        if (value.Length < 4)
            throw new ArgumentException("Слишком короткий логин. Логин должен быть длиннее 3х символов");
        
        // Проверка прочих парольных политик
        
        return new Login(value);
    }

    public override string ToString()
        => _login;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return _login;
    }
    
    public static bool operator ==(Login left, Login right) => EqualOperator(left, right);

    public static bool operator !=(Login left, Login right) => NotEqualOperator(left, right);
}