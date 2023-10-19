using System.ComponentModel.DataAnnotations;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Domain.Friends;
using UserService.Domain.User;

namespace UserService.Application.Services;

public class FriendsService
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IDataQueryService _dataQueryService;

    public FriendsService(IFriendshipRepository friendshipRepository, IDataQueryService dataQueryService)
    {
        _friendshipRepository = friendshipRepository;
        _dataQueryService = dataQueryService;
    }

    public async Task<IEnumerable<string>> GetFriends(UserId userId)
    {
        return await _dataQueryService.FindFriends(userId);
    }

    public async Task Add(UserId userId, UserId friendId)
    {
        var id = new FriendshipId(userId, friendId);
        var friendship = Friendship.New(id);
        await _friendshipRepository.Add(friendship);
    }
    
    public async Task Delete(UserId userId, UserId friendId)
    {
        var id = new FriendshipId(userId, friendId);
        var friendship = await _friendshipRepository[id];
        await _friendshipRepository.Remove(friendship);
    }
}