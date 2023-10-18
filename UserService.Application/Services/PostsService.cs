using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Posts;
using UserService.Domain.User;

namespace UserService.Application.Services;

public class PostsService
{
    private readonly IPostRepository _postRepository;
    private readonly IDataQueryService _dataQueryService;

    public PostsService(IPostRepository postRepository, IDataQueryService dataQueryService)
    {
        _postRepository = postRepository;
        _dataQueryService = dataQueryService;
    }

    public async Task<IEnumerable<PostViewModel>> FindPosts(UserId userId)
    {
        return await _dataQueryService.GetPosts(userId);
    }

    public async Task<PostViewModel> GetPost(PostId postId)
    {
        return await _dataQueryService.FindPost(postId);
    }

    public async Task<PostId> Create(UserId userId, string text)
    {
        var post = Post.New(userId, text);
        await _postRepository.Add(post);
        return post.Id;
    }

    public async Task Update(UserId userId, PostId postId, string text)
    {
        var post = await _postRepository[postId];
        if (post.Author != userId)
            throw new UnauthorizedAccessException($"User {userId} is not allowed to update post {postId}");

        post.Text = text;
    }

    public async Task Delete(UserId userId, PostId postId)
    {
        var post = await _postRepository[postId];
        if (post.Author != userId)
            throw new UnauthorizedAccessException($"User {userId} is not allowed to delete post {postId}");

        await _postRepository.Remove(post);
    }
}