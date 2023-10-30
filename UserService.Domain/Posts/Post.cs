using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Posts;

public class Post : Entity<PostId>
{
    private string _text;

    public string Text
    {
        get => _text;
        set
        {
            if(_text == value)
                return;

            _text = value;
            RegisterEvent(new PostChangedDomainEvent(DomainEventId.New(), DateTime.Now, Id, Author));
        }
    }
    public UserId Author { get; }
    
    public DateTime CreatedAt { get; }
    public Post(PostId id, UserId author, string text, DateTime createdAt) : base(id)
    {
        _text = text;
        CreatedAt = createdAt;
        Author = author ?? throw new ArgumentNullException(nameof(author));
    }

    public static Post New(UserId author, string text) => new (PostId.New(), author, text, DateTime.Now);
}