using UserService.Domain.Friends;
using UserService.Domain.User;

namespace UserService.Application.Interfaces;

public interface IFriendshipRepository : IRepository<Friendship, FriendshipId>
{
    
}