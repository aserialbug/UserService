namespace UserService.Domain.Common;

public abstract class Entity<TId> where TId : BaseId
{
    protected Entity(TId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    public TId Id { get; }
}