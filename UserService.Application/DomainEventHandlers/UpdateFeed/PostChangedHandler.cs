using Microsoft.Extensions.Logging;
using UserService.Domain.Posts;

namespace UserService.Application.DomainEventHandlers.UpdateFeed;

public class PostChangedHandler : BaseDomainEventHandler<PostChangedDomainEvent>
{
    public PostChangedHandler(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }

    protected override async Task ProtectedHandle(PostChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}