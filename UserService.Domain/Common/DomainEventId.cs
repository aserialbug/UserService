using MassTransit;

namespace UserService.Domain.Common;

public class DomainEventId : BaseId
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
            throw new ArgumentException($"Value {value} is invalid DomainEventId");
        
        var id = new NewId(idTmp[1]);
        
        return new DomainEventId(id);
    }

    protected override string GetIdPrefix()
        => nameof(DomainEventId);
}