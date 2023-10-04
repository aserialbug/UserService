using System.Globalization;
using System.Runtime.CompilerServices;
using UserService.Application.Models;

namespace UserService.Utils;

public static class IFormFileExtensions
{
    public static async IAsyncEnumerable<RegisterCommand> ReadUsers(this IFormFile formFile)
    {
        await using var stream = formFile.OpenReadStream();
        using var reader = new StreamReader(stream);
        while (await reader.ReadLineAsync() is { } current)
        {
            yield return ReadCommand(current);
        }
    }

    private static RegisterCommand ReadCommand(string value)
    {
        var sectors = value.Split(';');
        if (sectors.Length != 6)
            throw new ArgumentException("Invalid batch register CSV file");
        
        return new RegisterCommand
        {
            Second_name = sectors[0],
            First_name = sectors[1],
            Birthdate = DateTime.ParseExact(sectors[2].AsSpan(), "dd.MM.yyyy", CultureInfo.InvariantCulture),
            City = sectors[3],
            Biography = sectors[4],
            Password = sectors[5]
        };
    }
}