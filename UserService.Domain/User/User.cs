using UserService.Domain.Base;

namespace UserService.Domain.User;

public class User : Entity<UserId>
{
    public Login Login { get; }
    public Password Password { get; }
    
    public User(UserId id, Login login, Password password) : base(id)
    {
        Login = login;
        Password = password;
    }
}