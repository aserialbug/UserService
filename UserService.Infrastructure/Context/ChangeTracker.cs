using UserService.Domain.Common;

namespace UserService.Infrastructure.Context;

public class ChangeTracker
{
    public EntityState State { get; private set; }
    public Entity Entity { get; }
    
    private ChangeTracker(Entity entity, EntityState state)
    {
        Entity = entity;
        State = state;
    }

    public void Deleted()
        => State = EntityState.Deleted;

    public static ChangeTracker NewEntity(Entity entity)
        => new (entity, EntityState.New);
    
    public static ChangeTracker CleanEntity(Entity entity)
        => new (entity, EntityState.Clean);
    
    public static ChangeTracker DeletedEntity(Entity entity)
        => new (entity, EntityState.Deleted);
    
    public enum EntityState
    {
        New,
        Clean,
        Dirty,
        Deleted
    }
}