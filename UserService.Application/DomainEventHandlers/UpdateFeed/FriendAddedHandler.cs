using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
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

    protected override async Task ProtectedHandle(FriendshipCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            _feedService.RebuildFeed(notification.From),
            _feedService.RebuildFeed(notification.To));
    }
}