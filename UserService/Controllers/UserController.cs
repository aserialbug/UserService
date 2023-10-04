using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Application.Services;
using UserService.Domain.User;
using UserService.Utils;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController
{
    private readonly PersonService _personService;
    private readonly RegisterService _registerService;
    private readonly IDataQueryService _queryService;

    public UserController(PersonService personService, RegisterService registerService, IDataQueryService queryService)
    {
        _personService = personService;
        _registerService = registerService;
        _queryService = queryService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<RegisterResponse> Register(RegisterCommand command)
    {
        var userId = await _registerService.Register(command);
        return new RegisterResponse { User_id = userId.ToString() };
    }
    
    [HttpPost("register/batch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task BatchRegister(IFormFile formFile)
    {
        var commands = formFile.ReadUsers();
        await _registerService.BatchRegister(commands);
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
    
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<UserViewModel[]> Search([FromQuery(Name = "first_name")] string firstName, [FromQuery(Name = "last_name")] string lastName)
    {
        return await _queryService.Search(firstName, lastName);
    }
}