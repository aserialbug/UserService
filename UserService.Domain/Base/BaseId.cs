using System;
using System.Collections.Generic;

namespace UserService.Domain.Base;

public abstract class BaseId : ValueObject
{
    private readonly string _id;

    protected BaseId(string id)
    {
        if(string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));
        
        _id = id;
    }

    public override string ToString()
        => _id;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return _id;
    }

    public static bool operator ==(BaseId left, BaseId right) => EqualOperator(left, right);

    public static bool operator !=(BaseId left, BaseId right) => NotEqualOperator(left, right);
}