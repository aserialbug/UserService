using Npgsql;
using NpgsqlTypes;
using UserService.Application.Exceptions;
using UserService.Domain.Friends;
using UserService.Domain.User;

namespace UserService.Infrastructure.Context;

public static class NpgsqlDataSourceFriendsExtensions
{
    private const string AddFriendshipSql =
        "insert into friendships (friend_from, friend_to, created) values " +
        "(@friend_from, @friend_to, @created)";
    
    private const string DeleteFriendshipSql = 
        "delete from friendships where smaller_first(friend_from, friend_to) = smaller_first(@friend_from, @friend_to)";
    
    private const string GetFriendshipSql = 
        "select friend_from, friend_to, created from friendships where " +
        "smaller_first(friend_from, friend_to) = smaller_first(@friend_from, @friend_to)";
    
    private const string FindFriendsSql = 
        "select friend_from, friend_to from friendships where " +
        @"smaller_first(friend_from, friend_to) && ARRAY[@userId]";
    
    public static async Task AddFriendship(this NpgsqlDataSource dataSource, Friendship friendship)
    {
        await using var addFriendshipCommand = dataSource.CreateCommand(AddFriendshipSql);
        addFriendshipCommand.Parameters.AddWithValue("friend_from", NpgsqlDbType.Uuid, friendship.Id.From.ToGuid());
        addFriendshipCommand.Parameters.AddWithValue("friend_to", NpgsqlDbType.Uuid, friendship.Id.To.ToGuid());
        addFriendshipCommand.Parameters.AddWithValue("created", NpgsqlDbType.Timestamp, friendship.CreatedAt);
        await addFriendshipCommand.ExecuteNonQueryAsync();
    }
    
    public static async Task RemoveFriendship(this NpgsqlDataSource dataSource, FriendshipId friendshipId)
    {
        await using var removeFriendshipCommand = dataSource.CreateCommand(DeleteFriendshipSql);
        removeFriendshipCommand.Parameters.AddWithValue("friend_from", NpgsqlDbType.Uuid, friendshipId.From.ToGuid());
        removeFriendshipCommand.Parameters.AddWithValue("friend_to", NpgsqlDbType.Uuid, friendshipId.To.ToGuid());
        await removeFriendshipCommand.ExecuteNonQueryAsync();
    }
    
    public static async Task<Friendship> GetFriendship(this NpgsqlDataSource dataSource, FriendshipId friendshipId)
    {
        await using var findFriendshipCommand = dataSource.CreateCommand(GetFriendshipSql);
        findFriendshipCommand.Parameters.AddWithValue("friend_from", NpgsqlDbType.Uuid, friendshipId.From.ToGuid());
        findFriendshipCommand.Parameters.AddWithValue("friend_to", NpgsqlDbType.Uuid, friendshipId.To.ToGuid());
        await using var reader = await findFriendshipCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            throw new NotFoundException($"Friendship between {friendshipId.From} and {friendshipId.To} was not found");

        var from = UserId.FromGuid(reader.GetGuid(0));
        var to = UserId.FromGuid(reader.GetGuid(1));
        var created = reader.GetDateTime(2);

        return new Friendship(new FriendshipId(from, to), created);
    }

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