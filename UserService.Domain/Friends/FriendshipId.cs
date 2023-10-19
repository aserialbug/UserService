using UserService.Domain.Common;
using UserService.Domain.User;

namespace UserService.Domain.Friends;

public class FriendshipId : BaseId
{
    public UserId From { get; }
    public UserId To { get; }
    
    public FriendshipId(UserId from, UserId to)
    {
        From = from ?? throw new ArgumentNullException(nameof(from));
        To = to ?? throw new ArgumentNullException(nameof(to));
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return From;
        yield return To;
    }
}