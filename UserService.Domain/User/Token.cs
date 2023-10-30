using System;
using System.Collections.Generic;
using UserService.Domain.Common;
using UserService.Domain.Common;

namespace UserService.Domain.User;

public class Token : ValueObject
{
    private readonly string _token;

    public Token(string token)
    {
        _token = token;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return _token;
    }

    public override string ToString()
        => _token;

    public static Token New() => new (Guid.NewGuid().ToString());
    
    public static bool operator ==(Token left, Token right) => EqualOperator(left, right);

    public static bool operator !=(Token left, Token right) => NotEqualOperator(left, right);
}