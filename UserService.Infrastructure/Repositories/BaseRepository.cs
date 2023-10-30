using Npgsql;
using UserService.Infrastructure.Context;

namespace UserService.Infrastructure.Repositories;

internal abstract class BaseRepository
{
    protected EntitiesContext Context { get; }
    
    protected BaseRepository(EntitiesContext entitiesContext)
    {
        Context = entitiesContext;
    }
}