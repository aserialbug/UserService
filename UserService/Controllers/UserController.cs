using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Application.Services;
using UserService.Domain.User;
using UserService.Utils;

namespace UserService.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class UsersController : BaseController
{
    private readonly RegisterService _registerService;
    private readonly IDataQueryService _queryService;

    public UsersController(RegisterService registerService, IDataQueryService queryService)
    {
        _registerService = registerService;
        _queryService = queryService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<RegisterResponse> Register(RegisterCommand command)
    {
        var userId = await _registerService.Register(command);
        return new RegisterResponse { User_id = userId.ToString() };
    }
    
    [HttpPost("batch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task BatchRegister(IFormFile formFile)
    {
        var commands = formFile.ReadUsers();
        await _registerService.BatchRegister(commands);
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<PersonViewModel> GetPersonById([FromRoute] string userId)
    {
        var id = UserId.Parse(userId);
        var person = await _queryService.FindPerson(id);
        if(person == null)
            throw new NotFoundException($"Person with id={userId} was not found");
        
        return person;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<PersonViewModel[]> Search([FromQuery(Name = "first_name")] string firstName, [FromQuery(Name = "last_name")] string lastName)
    {
        var persons = await _queryService.SearchPersons(firstName, lastName);
        return persons.ToArray();
    }
}