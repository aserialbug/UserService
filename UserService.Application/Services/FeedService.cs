using UserService.Application.Interfaces;
using UserService.Application.Models;
using UserService.Domain.Posts;
using UserService.Domain.User;

namespace UserService.Application.Services;

public class FeedService
{
    private const int MaxSingleRequestFeedCount = 50;
    
    private readonly IDataQueryService _dataQueryService;
    private readonly IFeedCacheService _feedCacheService;

    public FeedService(IDataQueryService dataQueryService, IFeedCacheService feedCacheService)
    {
        _dataQueryService = dataQueryService;
        _feedCacheService = feedCacheService;
    }

    public async Task<PagedResult<PostViewModel>> GetPagedFeedAsync(UserId userId, int? offset, int? count, CancellationToken token = default)
    {
        var getCount = count ?? MaxSingleRequestFeedCount;
        switch (getCount)
        {
            case 0:
                return new PagedResult<PostViewModel>(Array.Empty<PostViewModel>());
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(count), "count must be positive value");
            case > MaxSingleRequestFeedCount:
                throw new ArgumentOutOfRangeException(nameof(count), $"cannot request more than {MaxSingleRequestFeedCount} records at a time");
        }

        var getOffset = Math.Max(0, offset ?? 0);
        
        var data = await _feedCacheService.GetFeedDataAsync(userId.ToString(), getOffset, getCount + 1);
        IEnumerable<PostViewModel> feed;
        if (data.Length > count)
        {
            feed = await _dataQueryService.GetPostsByIds(data.Take(count.Value));
            return new PagedResult<PostViewModel>(feed.ToArray(), offset + count);
        }
        else
        {
            feed = await _dataQueryService.GetPostsByIds(data);
            return new PagedResult<PostViewModel>(feed.ToArray());
        }
    }

    public async Task RebuildFeedAsync(UserId userId, CancellationToken token = default)
    {
        var userStr = userId.ToString();
        var feed = await _dataQueryService.BuildFeed(userStr);
        await _feedCacheService.ClearFeedAsync(userStr);
        foreach (var viewModel in feed)
        {
            await _feedCacheService.AddPostAsync(userStr, viewModel.PostId);
        }
    }

    public async Task AddPostToFeedsAsync(PostId postId, UserId authorId, CancellationToken token = default)
    {
        var friends = await _dataQueryService.FindFriends(authorId.ToString());
        foreach (var friendId in friends)
        {
            await _feedCacheService.AddPostAsync(friendId, postId.ToString());
        }
    }

    public async Task DeletePost(PostId postId)
    {
        await _feedCacheService.DeletePostAsync(postId.ToString());
        await Task.CompletedTask;
    }
}