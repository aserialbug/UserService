using MassTransit;

namespace UserService.Domain.Common;

public sealed class DomainEventId : SimpleId
{
    private DomainEventId(NewId id) : base(id)
    {
    }
    
    public static DomainEventId New() => new (NewId.Next());
    
    public static DomainEventId Parse(string value)
    {
        var idTmp = value.Split('_');
        
        if (idTmp.Length != 2 ||
            !idTmp[0].Equals(nameof(DomainEventId)))
            throw new ArgumentException($"Value {value} is invalid {nameof(DomainEventId)}");
        
        var id = new NewId(idTmp[1]);
        
        return new DomainEventId(id);
    }
}