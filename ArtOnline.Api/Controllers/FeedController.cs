using ArtOnline.Database.Repository.Enums;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtOnline.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class FeedController(ILogger<FeedController> logger, IUserService userService, IFeedService feedService)
    : AuthorizedController(logger, userService)
{
    [HttpGet]
    public async Task<IActionResult> GetFeed(
        [FromQuery] PaginationSearchQueryParams pagination, 
        [FromQuery] ArtworkOrderEnum sort = ArtworkOrderEnum.Random)
    {
        var response = await feedService.GetArtworkFeed(pagination, sort);
        return Ok(response);
    }
}