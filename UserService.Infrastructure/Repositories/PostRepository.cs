using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Posts;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(PostgresContext postgreSqlContext) : base(postgreSqlContext)
    {
    }

    public Task<Post> this[PostId id] => GetById(id);
    
    private async Task<Post> GetById(PostId id)
    {
        var post  = await DataSource.GetPost(id);
        return post;
    }

    public async Task Add(Post entity)
    {
        await DataSource.AddPost(entity);
    }

    public async Task Remove(Post entity)
    {
        await DataSource.RemovePost(entity.Id);
    }
}