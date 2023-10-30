using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Services;
using UserService.Domain.User;
using UserService.Filters;

namespace UserService.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FriendsController : BaseController
{
    private readonly FriendsService _friendsService;

    public FriendsController(FriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<string[]> GetFriends()
    {
        var id = GetAuthenticatedUser() 
                 ?? throw new UnauthorizedAccessException();
        var friends = await _friendsService.GetFriends(id);
        return friends.Select(i => i.ToString()).ToArray();
    }

    [HttpPost("{friendId}")]
    [BusinessTransaction]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task AddFriend([FromRoute] string friendId)
    {
        var id = GetAuthenticatedUser() 
                 ?? throw new UnauthorizedAccessException();
        var friend = UserId.Parse(friendId);
        await _friendsService.Add(id, friend);
    }
    
    [HttpDelete("{friendId}")]
    [BusinessTransaction]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task DeleteFriend([FromRoute] string friendId)
    {
        var id = GetAuthenticatedUser() 
                 ?? throw new UnauthorizedAccessException();
        var friend = UserId.Parse(friendId);
        await _friendsService.Delete(id, friend);
    }
}