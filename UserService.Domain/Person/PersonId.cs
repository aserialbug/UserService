using MassTransit;
using UserService.Domain.Common;

namespace UserService.Domain.Person;

public class PersonId : SimpleId
{
    public PersonId(NewId id) : base(id)
    {
    }
    
    public static PersonId New()
    {
        return new(NewId.Next());
    }

    public static PersonId Parse(string value)
    {
        var idTmp = value.Split('_');

        if (idTmp.Length != 2 ||
            !idTmp[0].Equals(nameof(PersonId)))
            throw new ArgumentException($"Value {value} is invalid {nameof(PersonId)}");
        
        var id = new NewId(idTmp[1]);
        
        return new PersonId(id);
    }
    
    public static PersonId FromGuid(Guid value)
    {
        return new(NewId.FromGuid(value));
    }
}