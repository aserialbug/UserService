using Microsoft.Extensions.Logging;
using UserService.Domain.Posts;

namespace UserService.Application.DomainEventHandlers.UpdateFeed;

public class PostDeletedHandler : BaseDomainEventHandler<PostDeletedDomainEvent>
{
    public PostDeletedHandler(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }

    protected override async Task ProtectedHandle(PostDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}