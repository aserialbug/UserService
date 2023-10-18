using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Models;
using UserService.Application.Services;
using UserService.Domain.Posts;

namespace UserService.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostsController : BaseController
{
    private readonly PostsService _postsService;

    public PostsController(PostsService postsService)
    {
        _postsService = postsService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<string> Create([FromBody] PostCommand postCommand)
    {
        var id = GetAuthenticatedUser() 
                 ?? throw new UnauthorizedAccessException();
        var postId = await _postsService.Create(id, postCommand.Text);
        return postId.ToString();
    }
    
    [HttpGet("{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<PostViewModel> GetPost([FromRoute] string postId)
    {
        var id = GetAuthenticatedUser() 
                 ?? throw new UnauthorizedAccessException();
        var post = PostId.Parse(postId);
        return await _postsService.GetPost(post);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<PostViewModel[]> Get()
    {
        var id = GetAuthenticatedUser() 
                 ?? throw new UnauthorizedAccessException();
        var posts = await _postsService.FindPosts(id);
        return posts.ToArray();
    }
    
    [HttpPut("{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task Update([FromRoute]string postId, [FromBody] PostCommand postCommand)
    {
        var id = GetAuthenticatedUser() 
                 ?? throw new UnauthorizedAccessException();
        var post = PostId.Parse(postId);
        await _postsService.Update(id, post, postCommand.Text);
    }

    [HttpDelete("{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task Delete([FromRoute] string postId)
    {
        var id = GetAuthenticatedUser() 
                 ?? throw new UnauthorizedAccessException();
        var post = PostId.Parse(postId);
        await _postsService.Delete(id, post);
    }
}