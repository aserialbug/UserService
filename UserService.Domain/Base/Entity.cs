using System;

namespace UserService.Domain.Base;

public class Entity<TId> where TId : BaseId
{
    public TId Id { get; }

    protected Entity(TId id)
    {
        Id = id
            ?? throw new ArgumentNullException(nameof(id));
    }
}