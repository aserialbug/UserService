using Microsoft.Extensions.Logging;
using UserService.Application.Services;
using UserService.Domain.Posts;

namespace UserService.Application.DomainEventHandlers.UpdateFeed;

public class PostDeletedHandler : BaseDomainEventHandler<PostDeletedDomainEvent>
{
    private readonly FeedService _feedService;
    public PostDeletedHandler(ILoggerFactory loggerFactory, FeedService feedService) : base(loggerFactory)
    {
        _feedService = feedService;
    }

    protected override async Task ProtectedHandle(PostDeletedDomainEvent notification, CancellationToken cancellationToken = default)
    {
        await _feedService.DeletePost(notification.PostId);
    }
}