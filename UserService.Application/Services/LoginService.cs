using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Application.Utils;
using UserService.Domain.User;

namespace UserService.Application.Services;

public class LoginService
{
    private readonly IDataQueryService _dataQuery;
    private readonly IHashingService _encryptionService;
    private readonly ITokenService _tokenGeneratorService;

    public LoginService(IHashingService encryptionService,
        ITokenService tokenGeneratorService,
        IDataQueryService dataQuery)
    {
        _encryptionService = encryptionService;
        _tokenGeneratorService = tokenGeneratorService;
        _dataQuery = dataQuery;
    }

    public async Task<Token> Login(UserId userId, ClearTextPassword clearTextPassword)
    {
        // Пытаемся найти пароль пользователя в БД
        var user = await _dataQuery.FindUser(userId);
        // Если нашли - берем его Соль и хэшируем переданный пароль для сравнения,
        // а если не нашли, то соль для шифрования пароля будет сгенерированна случайным 
        // образом и совпадение хэшей будет очень и очень маловероятным
        var encrypted = await _encryptionService.Encrypt(clearTextPassword, user?.Password.Salt);
        bool authenticated = false;
        if (user == null)
        {
            // Пользователь не найден, но для сохранения общего времени
            // выполнения метода делаем пустое сравнение
            bool temp = encrypted == StaticRandomPassword.Value;
        }
        else
        {
            authenticated = user.Password == encrypted;
        }

        if (!authenticated)
            throw new NotFoundException("User with combination id/password was not found");

        return await _tokenGeneratorService.GetToken(user);
    }
}