using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Posts;

public class PostDeletedDomainEvent : PostDomainEvent
{
    public PostDeletedDomainEvent(DomainEventId id, DateTime createdAt, PostId postId, UserId authorId) : base(id, createdAt, postId, authorId)
    {
    }
}