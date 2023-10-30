using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

public delegate object ServiceFactory(Type type);

public static class ServiceFactoryExtensions
{
    public static IStorageAdapter<TEntity, TId> GetAdapter<TEntity, TId>(this ServiceFactory factory)
        where TEntity : Entity<TId> 
        where TId : BaseId
    {
        return (IStorageAdapter<TEntity, TId>)factory(typeof(IStorageAdapter<TEntity, TId>));
    }
}