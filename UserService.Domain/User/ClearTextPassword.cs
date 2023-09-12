using System.Text;
using UserService.Domain.Base;

namespace UserService.Domain.User;

public class ClearTextPassword : BaseString
{
    private ClearTextPassword(byte[] password) : base(password)
    {
    }

    public static ClearTextPassword Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));
        
        if (value.Length < 4)
            throw new ArgumentException("Пароль слишком короткий. Используйте пароль длинне 3х символов");

        return new ClearTextPassword(Encoding.UTF8.GetBytes(value));
    }
}