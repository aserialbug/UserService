using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Posts;
using UserService.Domain.User;

namespace UserService.Application.DomainEventHandlers.UpdateFeed;

public class PostCreatedHandler : BaseDomainEventHandler<PostCreatedDomainEvent>
{
    private readonly IDataQueryService _dataQueryService;
    private readonly IFeedCacheService _feedCacheService;

    public PostCreatedHandler(ILoggerFactory loggerFactory, IDataQueryService dataQueryService, IFeedCacheService feedCacheService) : base(loggerFactory)
    {
        _dataQueryService = dataQueryService;
        _feedCacheService = feedCacheService;
    }

    protected override async Task ProtectedHandle(PostCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var friends = _dataQueryService.FindFriends(notification.AuthorId);
        var post = _dataQueryService.FindPost(notification.PostId);
        await Task.WhenAll(friends, post);

        await _feedCacheService.AddPost(friends.Result.Select(UserId.Parse).Append(notification.AuthorId), post.Result);
    }
}