using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Posts;

public class PostCreatedDomainEvent : PostDomainEvent
{
    public PostCreatedDomainEvent(DomainEventId id, DateTime createdAt, PostId postId, UserId authorId) : base(id, createdAt, postId, authorId)
    {
    }
}