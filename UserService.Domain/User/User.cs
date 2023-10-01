using UserService.Domain.Common;

namespace UserService.Domain.User;

public class User : Entity<UserId>
{
    public User(UserId id, Login login, Password password) : base(id)
    {
        Login = login;
        Password = password;
    }

    public Login Login { get; }
    public Password Password { get; }
}