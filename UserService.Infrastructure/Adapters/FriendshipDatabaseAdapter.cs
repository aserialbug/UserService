using NpgsqlTypes;
using UserService.Application.Exceptions;
using UserService.Domain.Friends;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Adapters;

public class FriendshipDatabaseAdapter : IStorageAdapter<Friendship, FriendshipId>
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
    
    public async Task<Friendship> GetAsync(FriendshipId id, DatabaseTransaction transaction)
    {
        await using var findFriendshipCommand = transaction.CreateCommand(GetFriendshipSql);
        findFriendshipCommand.Parameters.AddWithValue("friend_from", NpgsqlDbType.Uuid, id.From.ToGuid());
        findFriendshipCommand.Parameters.AddWithValue("friend_to", NpgsqlDbType.Uuid, id.To.ToGuid());
        await using var reader = await findFriendshipCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            throw new NotFoundException($"Friendship between {id.From} and {id.To} was not found");

        var from = UserId.FromGuid(reader.GetGuid(0));
        var to = UserId.FromGuid(reader.GetGuid(1));
        var created = reader.GetDateTime(2);

        return new Friendship(new FriendshipId(from, to), created);
    }

    public async Task AddAsync(Friendship entity, DatabaseTransaction transaction)
    {
        await using var addFriendshipCommand = transaction.CreateCommand(AddFriendshipSql);
        addFriendshipCommand.Parameters.AddWithValue("friend_from", NpgsqlDbType.Uuid, entity.Id.From.ToGuid());
        addFriendshipCommand.Parameters.AddWithValue("friend_to", NpgsqlDbType.Uuid, entity.Id.To.ToGuid());
        addFriendshipCommand.Parameters.AddWithValue("created", NpgsqlDbType.Timestamp, entity.CreatedAt);
        await addFriendshipCommand.ExecuteNonQueryAsync();
    }

    public Task UpdateAsync(Friendship entity, DatabaseTransaction transaction)
    {
        return Task.FromException(new InvalidOperationException("Updating friendship entity is currently not supported"));
    }

    public async Task DeleteAsync(FriendshipId id, DatabaseTransaction transaction)
    {
        await using var removeFriendshipCommand = transaction.CreateCommand(DeleteFriendshipSql);
        removeFriendshipCommand.Parameters.AddWithValue("friend_from", NpgsqlDbType.Uuid, id.From.ToGuid());
        removeFriendshipCommand.Parameters.AddWithValue("friend_to", NpgsqlDbType.Uuid, id.To.ToGuid());
        await removeFriendshipCommand.ExecuteNonQueryAsync();
    }
}