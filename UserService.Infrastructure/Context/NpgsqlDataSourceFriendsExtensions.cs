using Npgsql;
using UserService.Domain.Friends;
using UserService.Domain.User;

namespace UserService.Infrastructure.Context;

public static class NpgsqlDataSourceFriendsExtensions
{
    public static async Task AddFriendship(this NpgsqlDataSource dataSource, Friendship friendship)
    {
    }
    
    public static async Task RemoveFriendship(this NpgsqlDataSource dataSource, FriendshipId friendshipId)
    {
    }
    
    public static async Task<Friendship> GetFriendship(this NpgsqlDataSource dataSource, FriendshipId friendshipId)
    {
        return null;
    }

    public static async Task<IEnumerable<string>> FindFriends(this NpgsqlDataSource dataSource, UserId userId)
    {
        return Enumerable.Empty<string>();
    }
}