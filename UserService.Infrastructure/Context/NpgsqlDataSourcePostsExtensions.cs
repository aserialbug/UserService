using Npgsql;
using NpgsqlTypes;
using UserService.Application.Exceptions;
using UserService.Application.Models;
using UserService.Domain.Posts;
using UserService.Domain.User;

namespace UserService.Infrastructure.Context;

public static class NpgsqlDataSourcePostsExtensions
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
    
    private const string GetUserPostsCommandSql =
        "select id, author, created, content from posts where author = @authorId";
    
    public static async Task AddPost(this NpgsqlDataSource dataSource, Post post)
    {
        await using var addPostCommand = dataSource.CreateCommand(AddPostCommandSql);
        addPostCommand.Parameters.AddWithValue("id", NpgsqlDbType.Uuid, post.Id.ToGuid());
        addPostCommand.Parameters.AddWithValue("author", NpgsqlDbType.Uuid, post.Author.ToGuid());
        addPostCommand.Parameters.AddWithValue("created", NpgsqlDbType.Timestamp, post.CreatedAt);
        addPostCommand.Parameters.AddWithValue("content", NpgsqlDbType.Text, post.Text);
        await addPostCommand.ExecuteNonQueryAsync();
    }   
    
    public static async Task UpdatePost(this NpgsqlDataSource dataSource, Post post)
    {
        await using var updatePostCommand = dataSource.CreateCommand(UpdatePostCommandSql);
        updatePostCommand.Parameters.AddWithValue("new_content", NpgsqlDbType.Text, post.Text);
        updatePostCommand.Parameters.AddWithValue("postId", NpgsqlDbType.Uuid, post.Id.ToGuid());
        await updatePostCommand.ExecuteNonQueryAsync();
    }   
    
    public static async Task RemovePost(this NpgsqlDataSource dataSource, PostId postId)
    {
        await using var deletePostCommand = dataSource.CreateCommand(DeletePostCommandSql);
        deletePostCommand.Parameters.AddWithValue("postId", NpgsqlDbType.Uuid, postId.ToGuid());
        await deletePostCommand.ExecuteNonQueryAsync();
    }  
    
    public static async Task<Post> GetPost(this NpgsqlDataSource dataSource, PostId postId)
    {
        await using var findPostCommand = dataSource.CreateCommand(GetPostCommandSql);
        findPostCommand.Parameters.AddWithValue("postId", NpgsqlDbType.Uuid, postId.ToGuid());
        await using var reader = await findPostCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            throw new NotFoundException($"Post with id={postId} was not found");

        var id = PostId.FromGuid(reader.GetGuid(0));
        var author = UserId.FromGuid(reader.GetGuid(1));
        var created = reader.GetDateTime(2);
        var content = reader.GetString(3);

        return new Post(id, author, content, created);
    }
    
    public static async Task<PostViewModel?> FindPost(this NpgsqlDataSource dataSource, PostId postId)
    {
        await using var findPostCommand = dataSource.CreateCommand(GetPostCommandSql);
        findPostCommand.Parameters.AddWithValue("postId", NpgsqlDbType.Uuid, postId.ToGuid());
        await using var reader = await findPostCommand.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        var id = PostId.FromGuid(reader.GetGuid(0));
        var author = UserId.FromGuid(reader.GetGuid(1));
        var created = reader.GetDateTime(2);
        var content = reader.GetString(3);

        return new PostViewModel(id.ToString(), author.ToString(), content, created);
    }
    
    public static async Task<IEnumerable<PostViewModel>> GetUserPosts(this NpgsqlDataSource dataSource, UserId user)
    {
        await using var findFriendsCommand = dataSource.CreateCommand(GetUserPostsCommandSql);
        findFriendsCommand.Parameters.AddWithValue("authorId", NpgsqlDbType.Uuid, user.ToGuid());
        await using var reader = await findFriendsCommand.ExecuteReaderAsync();
        var result = new List<PostViewModel>();

        while (await reader.ReadAsync())
        {
            var id = PostId.FromGuid(reader.GetGuid(0));
            var author = UserId.FromGuid(reader.GetGuid(1));
            var created = reader.GetDateTime(2);
            var content = reader.GetString(3);

            result.Add(new PostViewModel(id.ToString(), author.ToString(), content, created));
        }

        return result;
    }
}