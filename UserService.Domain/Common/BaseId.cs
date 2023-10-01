﻿using MassTransit;

namespace UserService.Domain.Common;

public abstract class BaseId : ValueObject
{
    // NewId это тип сортируемых уникальных идентификатороу
    private readonly NewId _id;

    protected BaseId(NewId id)
    {
        if (id == NewId.Empty)
            throw new ArgumentNullException(nameof(id));

        _id = id;
    }

    protected abstract string GetIdPrefix();

    public override string ToString()
    {
        return $"{GetIdPrefix()}_{_id:N}";
    }

    public Guid ToGuid()
    {
        return _id.ToGuid();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return GetIdPrefix();
        yield return _id;
    }

    public static bool operator ==(BaseId left, BaseId right)
    {
        return EqualOperator(left, right);
    }

    public static bool operator !=(BaseId left, BaseId right)
    {
        return NotEqualOperator(left, right);
    }
}