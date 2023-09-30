using System;
using UserService.Domain.Common;

namespace UserService.Domain.User;

public class PasswordHash : BaseString
{
    public PasswordHash(byte[] bytes) : base(bytes)
    {
    }

    public static PasswordHash Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value));

        return new PasswordHash(Convert.FromBase64String(value));
    }
}