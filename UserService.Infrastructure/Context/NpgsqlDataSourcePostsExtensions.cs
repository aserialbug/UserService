using Npgsql;
using UserService.Application.Models;
using UserService.Domain.Posts;
using UserService.Domain.User;

namespace UserService.Infrastructure.Context;

public static class NpgsqlDataSourcePostsExtensions
{
    public static async Task AddPost(this NpgsqlDataSource dataSource, Post post)
    {
        await Task.CompletedTask;
    }   
    
    public static async Task UpdatePost(this NpgsqlDataSource dataSource, Post post)
    {
        await Task.CompletedTask;
    }   
    
    public static async Task RemovePost(this NpgsqlDataSource dataSource, PostId postId)
    {
        await Task.CompletedTask;
    }  
    
    public static async Task<Post> GetPost(this NpgsqlDataSource dataSource, PostId postId)
    {
        throw new NotImplementedException();
    }
    
    public static async Task<PostViewModel> FindPost(this NpgsqlDataSource dataSource, PostId postId)
    {
        throw new NotImplementedException();
    }
    
    public static async Task<IEnumerable<PostViewModel>> GetUserPosts(this NpgsqlDataSource dataSource, UserId user)
    {
        throw new NotImplementedException();
    }
}