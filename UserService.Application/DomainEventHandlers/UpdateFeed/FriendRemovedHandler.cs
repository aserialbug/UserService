using Microsoft.Extensions.Logging;
using UserService.Application.Services;
using UserService.Domain.Friends;

namespace UserService.Application.DomainEventHandlers.UpdateFeed;

public class FriendRemovedHandler : BaseDomainEventHandler<FriendshipDeletedDomainEvent>
{
    private readonly FeedService _feedService;

    public FriendRemovedHandler(ILoggerFactory loggerFactory, FeedService feedService) : base(loggerFactory)
    {
        _feedService = feedService;
    }

    protected override async Task ProtectedHandle(FriendshipDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            _feedService.RebuildFeed(notification.From),
            _feedService.RebuildFeed(notification.To));
    }
}