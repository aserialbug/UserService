namespace UserService.Application.Models;

public record UserViewModel
{
    public string First_name { get; init; }
    public string Second_name { get; init; }
    public int Age { get; init; }
    public DateTime Birthdate { get; init; }
    public string Biography { get; init; }
    public string City { get; init; }
}