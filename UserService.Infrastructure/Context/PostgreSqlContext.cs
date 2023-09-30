using Npgsql;
using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

internal class PostgreSqlContext
{
    private readonly NpgsqlDataSource _dataSource;
    public NpgsqlDataSource DataSource => _dataSource;

    private readonly HashSet<DomainEvent> _domainEvents = new();
    private readonly Dictionary<BaseId, ChangeTracker> _trackers = new();

    public PostgreSqlContext(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public void RegisterEvent(DomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (!_domainEvents.Add(domainEvent))
        {
            throw new InvalidOperationException(
                $"Domain event {domainEvent.GetType().Name} with id={domainEvent.Id} already exists");
        }
    }

    // public TEntity GetById<TEntity, TId>(TId id)
    //     where TEntity : Entity<TId>
    //     where TId : BaseId
    // {
    //     ArgumentNullException.ThrowIfNull(id);
    //     if (_trackers.TryGetValue(id, out var entity))
    //         return (TEntity)entity.Entity;
    //     
    //     // get from database and add to _trackers as clean entity
    //     return null;
    // }
    
    public void RegisterClean(Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!_trackers.TryAdd(entity.Id, ChangeTracker.CleanEntity(entity)))
            throw new InvalidOperationException($"Entity with Id={entity.Id} has already been tracked");
    }

    public void RegisterNew(Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (!_trackers.TryAdd(entity.Id, ChangeTracker.NewEntity(entity)))
            throw new InvalidOperationException($"Entity with Id={entity.Id} has already been tracked");
    }
    
    public void RegisterDeleted(Entity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        if (_trackers.TryGetValue(entity.Id, out var tracker))
        {
            tracker.Deleted();
            return;
        }

        _trackers.Add(entity.Id, ChangeTracker.DeletedEntity(entity));
    }
}