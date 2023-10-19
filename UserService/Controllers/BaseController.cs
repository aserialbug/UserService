using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UserService.Domain.User;

namespace UserService.Controllers;

public abstract class BaseController : ControllerBase
{
    protected UserId? GetAuthenticatedUser()
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId == null ? null : UserId.Parse(userId);
    }
}