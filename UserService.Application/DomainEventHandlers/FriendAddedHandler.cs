using Microsoft.Extensions.Logging;
using UserService.Domain.Friends;

namespace UserService.Application.DomainEventHandlers;

public class FriendAddedHandler : BaseDomainEventHandler<FriendshipCreatedDomainEvent>
{
    public FriendAddedHandler(ILogger<FriendAddedHandler> logger) : base(logger)
    {
    }

    protected override async Task ProtectedHandle(FriendshipCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}