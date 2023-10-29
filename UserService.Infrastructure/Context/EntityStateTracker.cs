using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

internal sealed class EntityStateTracker
{
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
    public Entity Entity { get; }


    private EntityStateTracker(Entity entity, EntityState state)
    {
        State = state;
        Entity = entity ?? throw new ArgumentNullException(nameof(entity));
    }

    public void Deleted()
        => State = EntityState.Deleted;

    public void Clean()
        => State = EntityState.Clean;

    public static EntityStateTracker NewEntity(Entity entity)
        => new (entity, EntityState.New);
    
    public static EntityStateTracker CleanEntity(Entity entity)
        => new (entity, EntityState.Clean);
    
    public static EntityStateTracker DeletedEntity(Entity entity)
        => new (entity, EntityState.Deleted);
}