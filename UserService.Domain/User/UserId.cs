using MassTransit;
using UserService.Domain.Common;

namespace UserService.Domain.User;

public class UserId : SimpleId
{
    private UserId(NewId id) : base(id)
    {
    }

    public static UserId New()
    {
        return new(NewId.Next());
    }

    public static UserId Parse(string value)
    {
        var idTmp = value.Split('_');

        if (idTmp.Length != 2 ||
            !idTmp[0].Equals(nameof(UserId)))
            throw new ArgumentException($"Value {value} is invalid {nameof(UserId)}");
        
        var id = new NewId(idTmp[1]);
        
        return new UserId(id);
    }
    
    public static UserId FromGuid(Guid value)
    {
        return new(NewId.FromGuid(value));
    }
}