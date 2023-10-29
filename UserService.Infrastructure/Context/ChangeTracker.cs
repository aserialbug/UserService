using UserService.Domain.Common;
using UserService.Infrastructure.Adapters;

namespace UserService.Infrastructure.Context;

public class ChangeTracker
{
    private readonly StorageAdapterWrapper _storageAdapter;

    public Entity Entity { get; }
    
    private EntityState _state;
    public EntityState State 
    {
        get
        {
            if (_state == EntityState.Clean && Entity.DomainEvents.Any())
                _state = EntityState.Dirty;

            return _state;
        }
        private set => _state = value;
    }

    private ChangeTracker(Entity entity, EntityState state, StorageAdapterWrapper storageAdapter)
    {
        Entity = entity ?? throw new ArgumentNullException(nameof(entity));
        _storageAdapter = storageAdapter ?? throw new ArgumentNullException(nameof(storageAdapter));
        State = state;
    }

    public async Task SaveChangesAsync(DatabaseTransaction transaction, DomainEventsDatabaseAdapter domainEventsDatabaseAdapter)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (domainEventsDatabaseAdapter == null) throw new ArgumentNullException(nameof(domainEventsDatabaseAdapter));

        if(State == EntityState.Clean)
            return;
        
        var task = State switch
        {
            EntityState.New => _storageAdapter.AddAsync(Entity, transaction),
            EntityState.Dirty => _storageAdapter.UpdateAsync(Entity, transaction),
            EntityState.Deleted => _storageAdapter.DeleteAsync(Entity, transaction),
            _ => Task.CompletedTask
        };
        await task;
        await domainEventsDatabaseAdapter.AddRange(Entity.DomainEvents, transaction);
        State = EntityState.Clean;
    }
    
    public void Deleted()
        => State = EntityState.Deleted;

    public static ChangeTracker New<TEntity, TId>(TEntity entity, IStorageAdapter<TEntity, TId> entityAdapter)
        where TEntity : Entity<TId> 
        where TId : BaseId
    {
        return new ChangeTracker(entity, EntityState.New, new StorageAdapterWrapper<TEntity,TId>(entityAdapter));
    }
    
    public static ChangeTracker Clean<TEntity, TId>(TEntity entity, IStorageAdapter<TEntity, TId> entityAdapter)
        where TEntity : Entity<TId> 
        where TId : BaseId
    {
        return new ChangeTracker(entity, EntityState.Clean, new StorageAdapterWrapper<TEntity,TId>(entityAdapter));
    }
    
    public static ChangeTracker Deleted<TEntity, TId>(TEntity entity, IStorageAdapter<TEntity, TId> entityAdapter)
        where TEntity : Entity<TId> 
        where TId : BaseId
    {
        return new ChangeTracker(entity, EntityState.Deleted, new StorageAdapterWrapper<TEntity,TId>(entityAdapter));
    }
}