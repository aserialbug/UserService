using UserService.Application.Interfaces;
using UserService.Domain.Posts;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal class PostRepository : BaseRepository, IPostRepository
{
    public Task<Post> this[PostId id] => DataSource.GetPost(id);
    
    public PostRepository(PostgreSqlContext postgreSqlContext) : base(postgreSqlContext)
    {
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