using System;

namespace UserService.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string? message = null) : base(message)
    {
    }
}