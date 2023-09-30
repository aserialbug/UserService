using UserService.Domain.Common;

namespace UserService.Application.Interfaces;

public interface IDomainEventHandler<TEvent> where TEvent: DomainEvent
{
    Task Handle(TEvent @event, CancellationToken token = default);
}