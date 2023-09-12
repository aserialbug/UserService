using Microsoft.AspNetCore.Mvc;
using UserService.Application.Models;
using UserService.Domain.User;

namespace UserService.Controllers;

[ApiController]
public class LoginController
{
    private readonly Application.Services.LoginService _userService;

    public LoginController(Application.Services.LoginService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Упрощенный процесс аутентификации путем передачи идентификатор пользователя и получения токена для дальнейшего прохождения авторизации
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("/login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<LoginResponse> Login(LoginCommand command)
    {
        var login = UserId.Parse(command.Id);
        var password = ClearTextPassword.Parse(command.Password);
        var token = await _userService.Login(login, password);
        return new LoginResponse { Token = token.ToString() };
    }
}