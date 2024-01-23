using Microsoft.Extensions.Logging;
using UserService.Application.Services;
using UserService.Domain.Posts;

namespace UserService.Application.DomainEventHandlers.UpdateFeed;

public class PostCreatedHandler : BaseDomainEventHandler<PostCreatedDomainEvent>
{
    private readonly FeedService _feedService;

    public PostCreatedHandler(ILoggerFactory loggerFactory, FeedService feedService) : base(loggerFactory)
    {
        _feedService = feedService;
    }

    protected override async Task ProtectedHandle(PostCreatedDomainEvent notification, CancellationToken cancellationToken = default)
    {
        await _feedService.AddPostToFeedsAsync(notification.PostId, notification.AuthorId, cancellationToken);
    }
}