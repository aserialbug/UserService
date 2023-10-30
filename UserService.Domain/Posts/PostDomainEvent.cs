using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Posts;

public abstract class PostDomainEvent : DomainEvent
{
    public PostId PostId { get; }
    public UserId AuthorId { get; }
    
    protected PostDomainEvent(DomainEventId id, DateTime createdAt, PostId postId, UserId authorId) : base(id, createdAt)
    {
        PostId = postId;
        AuthorId = authorId;
    }
}