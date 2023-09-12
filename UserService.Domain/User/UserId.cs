using UserService.Domain.Base;

namespace UserService.Domain.User;

public class UserId : BaseId
{
    private UserId(string id) : base(id)
    {
    }
    
    public static UserId New() => new ($"UserId_{Guid.NewGuid():N}");

    public static UserId Parse(string value)
    {
        var idTmp = value.Split('_');

        if (idTmp.Length != 2 ||
            !idTmp[0].Equals(nameof(UserId)) ||
            !Guid.TryParse(idTmp[1], out _))
            throw new ArgumentException($"Value {value} is invalid UserId");
        
        return new UserId(value);
    }
}