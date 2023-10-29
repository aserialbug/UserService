using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

public interface IStorageAdapter<TEntity, TId> 
    where TEntity : Entity<TId> 
    where TId : BaseId
{
    public Task<TEntity> GetAsync(TId id, DatabaseTransaction transaction);
    public Task AddAsync(TEntity entity, DatabaseTransaction transaction);
    public Task UpdateAsync(TEntity entity, DatabaseTransaction transaction);
    public Task DeleteAsync(TId id, DatabaseTransaction transaction);
}