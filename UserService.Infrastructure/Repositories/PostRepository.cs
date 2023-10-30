using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Posts;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal class PostRepository : BaseRepository, IPostRepository
{
    public Task<Post> this[PostId id] => Context.Posts.GetAsync(id);
    
    public PostRepository(EntitiesContext entitiesContext) : base(entitiesContext)
    {
    }

    public async Task Add(Post entity)
    {
        await Context.Posts.AddAsync(entity);
    }

    public async Task Remove(Post entity)
    {
        await Context.Posts.DeleteAsync(entity);
    }
}