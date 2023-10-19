using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Posts;

public class Post : Entity<PostId>
{
    public UserId Author { get; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; }
    public Post(PostId id, UserId author, string text, DateTime createdAt) : base(id)
    {
        Text = text;
        CreatedAt = createdAt;
        Author = author;
    }

    public static Post New(UserId author, string text) => new (PostId.New(), author, text, DateTime.Now);
}