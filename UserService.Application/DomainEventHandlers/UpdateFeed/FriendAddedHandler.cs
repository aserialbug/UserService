using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Friends;

namespace UserService.Application.DomainEventHandlers.UpdateFeed;

public class FriendAddedHandler : BaseDomainEventHandler<FriendshipCreatedDomainEvent>
{
    private readonly IDataQueryService _dataQueryService;
    private readonly IFeedCacheService _feedCacheService;
    
    public FriendAddedHandler(ILoggerFactory loggerFactory, IDataQueryService dataQueryService, IFeedCacheService feedCacheService) : base(loggerFactory)
    {
        _dataQueryService = dataQueryService;
        _feedCacheService = feedCacheService;
    }

    protected override async Task ProtectedHandle(FriendshipCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var from = _dataQueryService.BuildFeed(notification.From);
        var to = _dataQueryService.BuildFeed(notification.To);
        await Task.WhenAll(from, to);

        var addFrom = _feedCacheService.CacheFeed(notification.From, from.Result);
        var addTo = _feedCacheService.CacheFeed(notification.To, to.Result);
        await Task.WhenAll(addFrom, addTo);
    }
}