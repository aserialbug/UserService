using Microsoft.Extensions.Logging;
using UserService.Application.Services;
using UserService.Domain.Friends;

namespace UserService.Application.DomainEventHandlers.UpdateFeed;

public class FriendAddedHandler : BaseDomainEventHandler<FriendshipCreatedDomainEvent>
{
    private readonly FeedService _feedService;

    public FriendAddedHandler(ILoggerFactory loggerFactory, FeedService feedService) : base(loggerFactory)
    {
        _feedService = feedService;
    }

    protected override async Task ProtectedHandle(FriendshipCreatedDomainEvent notification, CancellationToken cancellationToken = default)
    {
        await Task.WhenAll(
            _feedService.RebuildFeedAsync(notification.From, cancellationToken),
            _feedService.RebuildFeedAsync(notification.To, cancellationToken));
    }
}