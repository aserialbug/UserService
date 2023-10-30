using NpgsqlTypes;
using UserService.Application.Exceptions;
using UserService.Domain.Posts;
using UserService.Domain.User;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Adapters;

public class PostsDatabaseAdapter : IStorageAdapter<Post, PostId>
{
    private const string AddPostCommandSql =
        "insert into posts (id, author, created, content) values " +
        "(@id, @author, @created, @content)";

    private const string UpdatePostCommandSql =
        "update posts set content = @new_content where id = @postId";
    
    private const string DeletePostCommandSql =
        "delete from posts where id = @postId";
    
    private const string GetPostCommandSql =
        "select id, author, created, content from posts where id = @postId";
    
    public async Task<Post> GetAsync(PostId id, DatabaseTransaction transaction)
    {
        await using var findPostCommand = transaction.CreateCommand(GetPostCommandSql);
        findPostCommand.Parameters.AddWithValue("postId", NpgsqlDbType.Uuid, id.ToGuid());
        await using var reader = await findPostCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            throw new NotFoundException($"Post with id={id} was not found");

        var postId = PostId.FromGuid(reader.GetGuid(0));
        var author = UserId.FromGuid(reader.GetGuid(1));
        var created = reader.GetDateTime(2);
        var content = reader.GetString(3);

        return new Post(postId, author, content, created);
    }

    public async Task AddAsync(Post entity, DatabaseTransaction transaction)
    {
        await using var addPostCommand = transaction.CreateCommand(AddPostCommandSql);
        addPostCommand.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, entity.Id.ToGuid());
        addPostCommand.Parameters.AddWithValue("author", NpgsqlDbType.Uuid, entity.Author.ToGuid());
        addPostCommand.Parameters.AddWithValue("created", NpgsqlDbType.Timestamp, entity.CreatedAt);
        addPostCommand.Parameters.AddWithValue("content", NpgsqlDbType.Text, entity.Text);
        await addPostCommand.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(Post entity, DatabaseTransaction transaction)
    {
        await using var updatePostCommand = transaction.CreateCommand(UpdatePostCommandSql);
        updatePostCommand.Parameters.AddWithValue("new_content", NpgsqlDbType.Text, entity.Text);
        updatePostCommand.Parameters.AddWithValue("postId", NpgsqlDbType.Uuid, entity.Id.ToGuid());
        await updatePostCommand.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(PostId id, DatabaseTransaction transaction)
    {
        await using var deletePostCommand = transaction.CreateCommand(DeletePostCommandSql);
        deletePostCommand.Parameters.AddWithValue("postId", NpgsqlDbType.Uuid, id.ToGuid());
        await deletePostCommand.ExecuteNonQueryAsync();
    }
}