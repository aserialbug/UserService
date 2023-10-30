using System.Diagnostics.CodeAnalysis;
using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

public class UnitOfWork
{
    private readonly Dictionary<BaseId, ChangeTracker> _entities = new();
    private readonly Dictionary<DomainEventId, DomainEventStateTracker> _domainEvents = new();
    public IEnumerable<ChangeTracker> Trackers => _entities.Values.ToArray();
    public IEnumerable<DomainEventStateTracker> EventTrackers => _domainEvents.Values.ToArray();

    public void RegisterClean<TEntity, TId>(TEntity entity, IStorageAdapter<TEntity, TId> storageAdapter)
        where TEntity : Entity<TId>
        where TId : BaseId
    {
        if (_entities.ContainsKey(entity.Id))
            throw new InvalidOperationException($"Entity {entity.Id} already tracked");

        _entities.Add(entity.Id, ChangeTracker.Clean(entity, storageAdapter));
    }

    public void RegisterNew<TEntity, TId>(TEntity entity, IStorageAdapter<TEntity, TId> storageAdapter)
        where TEntity : Entity<TId>
        where TId : BaseId
    {
        if (_entities.ContainsKey(entity.Id))
            throw new InvalidOperationException($"Entity {entity.Id} already tracked");

        _entities.Add(entity.Id, ChangeTracker.New(entity, storageAdapter));
    }

    public void RegisterDeleted<TEntity, TId>(TEntity entity, IStorageAdapter<TEntity, TId> storageAdapter)
        where TEntity : Entity<TId>
        where TId : BaseId
    {
        if (!_entities.TryGetValue(entity.Id, out var tracker))
        {
            _entities.Add(entity.Id, ChangeTracker.Deleted(entity, storageAdapter));
            return;
        }

        tracker.Deleted();
    }

    public bool TryGetEntity<TEntity, TId>(TId id, [MaybeNullWhen(false)] out TEntity entity)
        where TEntity : Entity<TId>
        where TId : BaseId
    {
        entity = null;
        if (_entities.TryGetValue(id, out var tracker))
        {
            entity = (TEntity)tracker.Entity;
            return true;
        }

        return false;
    }

    public void RegisterEvent(DomainEvent domainEvent)
    {
        if(_domainEvents.ContainsKey(domainEvent.Id))
            throw new InvalidOperationException($"DomainEvent {domainEvent.Id} already tracked");
        
        _domainEvents.Add(domainEvent.Id, DomainEventStateTracker.New(domainEvent));
    }
}