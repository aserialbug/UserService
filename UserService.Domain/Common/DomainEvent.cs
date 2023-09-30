namespace UserService.Domain.Common;

public abstract class DomainEvent
{
    public DomainEventId Id { get; }
    public DateTime CreatedAt { get; }
    
    protected DomainEvent(DomainEventId id, DateTime createdAt)
    {
        Id = id
             ?? throw new ArgumentNullException(nameof(id));
        CreatedAt = createdAt;
    }
}