using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Friends;

public class FriendshipId : BaseId
{
    public UserId Left { get; }
    public UserId Right { get; }
    
    public FriendshipId(UserId left, UserId right)
    {
        Left = left ?? throw new ArgumentNullException(nameof(left));
        Right = right ?? throw new ArgumentNullException(nameof(right));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Left;
        yield return Right;
    }
}