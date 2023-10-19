using UserService.Domain.Common;

namespace UserService.Domain.Friends;

public class Friendship : Entity<FriendshipId>
{
    public DateTime CreatedAt { get; }
    public Friendship(FriendshipId id, DateTime createdAt) : base(id)
    {
        CreatedAt = createdAt;
    }

    public static Friendship New(FriendshipId id)
        => new Friendship(id, DateTime.Now);
}