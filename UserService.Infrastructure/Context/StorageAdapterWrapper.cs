using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

public class StorageAdapterWrapper<TEntity, TId> : StorageAdapterWrapper
    where TEntity : Entity<TId>
    where TId : BaseId
{
    private readonly IStorageAdapter<TEntity, TId> _adapter;

    public StorageAdapterWrapper(IStorageAdapter<TEntity, TId> adapter)
    {
        _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
    }

    public override Task AddAsync(Entity entity, DatabaseTransaction transaction)
    {
        return _adapter.AddAsync((TEntity)entity, transaction);
    }

    public override Task UpdateAsync(Entity entity, DatabaseTransaction transaction)
    {
        return _adapter.UpdateAsync((TEntity)entity, transaction);
    }

    public override Task DeleteAsync(Entity entity, DatabaseTransaction transaction)
    {
        return _adapter.DeleteAsync((TId)entity.Id, transaction);
    }
}

public abstract class StorageAdapterWrapper
{
    public abstract Task AddAsync(Entity entity, DatabaseTransaction transaction);
    public abstract Task UpdateAsync(Entity entity, DatabaseTransaction transaction);
    public abstract Task DeleteAsync(Entity entity, DatabaseTransaction transaction);
}