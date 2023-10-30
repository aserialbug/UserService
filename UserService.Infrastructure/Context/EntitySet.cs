using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

public class EntitySet<TEntity, TId> 
    where TEntity : Entity<TId> 
    where TId : BaseId
{
    private readonly IStorageAdapter<TEntity, TId> _storageAdapter;
    private readonly DatabaseTransaction _transaction;
    private readonly UnitOfWork _unitOfWork;

    public EntitySet(IStorageAdapter<TEntity, TId> storageAdapter, UnitOfWork unitOfWork, DatabaseTransaction transaction)
    {
        _storageAdapter = storageAdapter ?? throw new ArgumentNullException(nameof(storageAdapter));
        _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<TEntity> GetAsync(TId id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        if (_unitOfWork.TryGetEntity(id, out TEntity entity)) 
            return entity;
        
        entity = await _storageAdapter.GetAsync(id, _transaction);
        _unitOfWork.RegisterClean(entity, _storageAdapter);

        return entity;
    }

    public Task AddAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        
        _unitOfWork.RegisterNew(entity, _storageAdapter);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        
        _unitOfWork.RegisterDeleted(entity, _storageAdapter);
        return Task.CompletedTask;
    }
}