using UserService.Application.Models;

namespace UserService.Infrastructure.Repositories;

public abstract class BaseRepository
{
    protected RequestContext RequestContext { get; }
    
    protected BaseRepository(RequestContext requestContext)
    {
        RequestContext = requestContext;
    }
}