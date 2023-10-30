using Npgsql;
using NpgsqlTypes;
using UserService.Application.Exceptions;
using UserService.Domain.Friends;
using UserService.Domain.User;

namespace UserService.Infrastructure.Context;

public static class NpgsqlDataSourceFriendsExtensions
{
    private const string FindFriendsSql = 
        "select friend_from, friend_to from friendships where " +
        @"smaller_first(friend_from, friend_to) && ARRAY[@userId]";
    

    public static async Task<IEnumerable<string>> FindFriends(this NpgsqlDataSource dataSource, UserId userId)
    {
        var guidId = userId.ToGuid();
        await using var findFriendsCommand = dataSource.CreateCommand(FindFriendsSql);
        findFriendsCommand.Parameters.AddWithValue("userId", NpgsqlDbType.Uuid, guidId);
        await using var reader = await findFriendsCommand.ExecuteReaderAsync();
        var result = new List<UserId>();

        while (await reader.ReadAsync())
        {
            var from = reader.GetGuid(0);
            result.Add(from == guidId ? UserId.FromGuid(reader.GetGuid(1)) : UserId.FromGuid(from));
        }

        return result.Select(id => id.ToString());
    }
}