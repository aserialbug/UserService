using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Domain.Common;

namespace UserService.Application.DomainEventHandlers;

public abstract class BaseDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : DomainEvent
{
    protected ILogger Logger { get; }

    protected BaseDomainEventHandler(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger(GetType());
    }

    public virtual async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        var eventName = typeof(TEvent).Name;
        try
        {
            Logger.LogInformation("Start handling domain event {Event}; Id={Id}", eventName, notification.Id);
            
            await ProtectedHandle(notification, cancellationToken);
            
            Logger.LogInformation("Successfully handled domain event {Event}; Id={Id}", eventName, notification.Id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error handling domain event {Event}; id ={Id}", eventName, notification.Id);
            throw;
        }
    }

    protected abstract Task ProtectedHandle(TEvent notification, CancellationToken cancellationToken = default);
}