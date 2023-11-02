using MediatR;
using Microsoft.Extensions.Logging;

namespace UserService.Application.DomainEventHandlers;

public abstract class BaseDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : INotification
{
    private ILogger<BaseDomainEventHandler<TEvent>> _logger;

    protected BaseDomainEventHandler(ILogger<BaseDomainEventHandler<TEvent>> logger)
    {
        _logger = logger;
    }

    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await ProtectedHandle(notification, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error handling domain event {Event}", typeof(TEvent).Name);
            throw;
        }
    }

    protected abstract Task ProtectedHandle(TEvent notification, CancellationToken cancellationToken);
}