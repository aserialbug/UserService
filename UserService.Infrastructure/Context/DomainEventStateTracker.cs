using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

public sealed class DomainEventStateTracker
{
    public DomainEventState State { get; private set; }
    public DomainEvent Event { get; }

    public DomainEventStateTracker(DomainEventState state, DomainEvent @event)
    {
        State = state;
        Event = @event ?? throw new ArgumentNullException(nameof(@event));
    }

    public static DomainEventStateTracker New(DomainEvent domainEvent)
        => new DomainEventStateTracker(DomainEventState.New, domainEvent);

    public void Saved()
        => State = DomainEventState.Saved;
}