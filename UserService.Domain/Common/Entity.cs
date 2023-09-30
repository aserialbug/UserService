namespace UserService.Domain.Common;

public abstract class Entity<TId> : Entity where TId : BaseId
{
    public new TId Id { get; }

    protected Entity(TId id) : base(id)
        => Id = id;
}

public abstract class Entity
{
    public BaseId Id { get; }
    
    private readonly HashSet<DomainEvent> _domainEvents = new();
    public IEnumerable<DomainEvent> DomainEvents => _domainEvents.ToArray();
    
    protected Entity(BaseId id)
    {
        Id = id 
            ?? throw new ArgumentNullException(nameof(id));
    }
    
    protected void RegisterEvent(DomainEvent domainEvent)
    {
        if (!_domainEvents.Add(
                domainEvent 
                ?? throw new ArgumentNullException(nameof(domainEvent))))
        {
            throw new InvalidOperationException(
                $"Domain event {domainEvent.GetType().Name} with id={domainEvent.Id} already exists");
        }
    }
}