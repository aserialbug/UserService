using UserService.Domain.Posts;

namespace UserService.Application.Interfaces;

public interface IPostRepository : IRepository<Post, PostId>
{
    
}