using Npgsql;
using NpgsqlTypes;
using UserService.Application.Exceptions;
using UserService.Application.Models;
using UserService.Domain.Posts;
using UserService.Domain.User;

namespace UserService.Infrastructure.Context;

public static class NpgsqlDataSourcePostsExtensions
{
    private const string GetPostCommandSql =
        "select id, author, created, content from posts where id = @postId";
    
    private const string GetUserPostsCommandSql =
        "select id, author, created, content from posts where author = @authorId order by created desc";
    
    private const string GetFeedCommandSql =
        "select id, author, created, content from posts where author = any(@users) order by created desc limit 1000";
    
    
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

    public static async Task<IEnumerable<PostViewModel>> BuildFeed(this NpgsqlDataSource dataSource, IEnumerable<string> users)
    {
        await using var getFeedCommand = dataSource.CreateCommand(GetFeedCommandSql);
        getFeedCommand.Parameters.AddWithValue("users", NpgsqlDbType.Array | NpgsqlDbType.Uuid, users.Select(s => UserId.Parse(s).ToGuid()).ToArray());
        await using var reader = await getFeedCommand.ExecuteReaderAsync();
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