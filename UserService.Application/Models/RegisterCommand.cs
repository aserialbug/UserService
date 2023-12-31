﻿namespace UserService.Application.Models;

public record RegisterCommand
{
    public string First_name { get; init; }
    public string Second_name { get; init; }
    public DateTime Birthdate { get; init; }
    public string Biography { get; init; }
    public string City { get; init; }
    public string Password { get; init; }
}