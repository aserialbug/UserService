using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Person;
using UserService.Domain.User;

namespace UserService.Application.Services;

public class RegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IHashingService _encryptionService;

    public RegisterService(IUserRepository userRepository, IPersonRepository personRepository, IHashingService encryptionService)
    {
        _userRepository = userRepository;
        _personRepository = personRepository;
        _encryptionService = encryptionService;
    }

    public async Task<UserId> Register(RegisterCommand registerCommand)
    {
        var userId = UserId.New();
        var login = Login.Parse($"{registerCommand.Second_name} {registerCommand.First_name}");
        var clearPassword = ClearTextPassword.Parse(registerCommand.Password);
        var password = await _encryptionService.Encrypt(clearPassword);
        var user = new User(userId, login, password);

        var person = new Person(
            userId,
            registerCommand.First_name,
            registerCommand.Second_name,
            registerCommand.Age,
            registerCommand.Birthdate,
            registerCommand.Biography,
            registerCommand.City);

        await Task.WhenAll(
                _userRepository.Add(user),
                _personRepository.Add(person));

        return userId;
    }
}