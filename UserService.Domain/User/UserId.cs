using System;
using MassTransit;
using UserService.Domain.Common;

namespace UserService.Domain.User;

public class UserId : BaseId
{
    private UserId(NewId id) : base(id)
    {
    }
    
    public static UserId New() => new (NewId.Next());

    public static UserId Parse(string value)
    {
        var idTmp = value.Split('_');

        if (idTmp.Length != 2 ||
            !idTmp[0].Equals(nameof(UserId)))
            throw new ArgumentException($"Value {value} is invalid UserId");
        
        var id = new NewId(idTmp[1]);
        
        return new UserId(id);
    }

    protected override string GetIdPrefix()
        => nameof(UserId);
}