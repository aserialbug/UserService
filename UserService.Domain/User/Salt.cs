using System;
using UserService.Domain.Base;

namespace UserService.Domain.User;

public class Salt : BaseString
{
    public Salt(byte[] value) : base(value)
    {
    }

    public static Salt Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        return new Salt(Convert.FromBase64String(value));
    }
}