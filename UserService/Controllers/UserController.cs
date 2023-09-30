using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Models;
using UserService.Application.Services;
using UserService.Domain.User;
using UserService.Filters;
using UserService.Utils;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController
{
    private readonly PersonService _personService;
    private readonly RegisterService _registerService;

    public UserController(PersonService personService, RegisterService registerService)
    {
        _personService = personService;
        _registerService = registerService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    [BusinessTransaction]
    public async Task<RegisterResponse> Register(RegisterCommand command)
    {
        var userId = await _registerService.Register(command);
        return new RegisterResponse { User_id = userId.ToString() };
    }

    [HttpGet("get/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<UserViewModel> GetUserById([FromRoute] string id)
    {
        var userId = UserId.Parse(id);
        var person = await _personService.GetById(userId);
        return person.ToViewModel();
    }
}