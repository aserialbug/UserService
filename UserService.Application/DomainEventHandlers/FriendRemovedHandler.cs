using Microsoft.Extensions.Logging;
using UserService.Domain.Friends;

namespace UserService.Application.DomainEventHandlers;

public class FriendRemovedHandler : BaseDomainEventHandler<FriendshipDeletedDomainEvent>
{
    public FriendRemovedHandler(ILogger<FriendRemovedHandler> logger) : base(logger)
    {
    }

    protected override async Task ProtectedHandle(FriendshipDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}