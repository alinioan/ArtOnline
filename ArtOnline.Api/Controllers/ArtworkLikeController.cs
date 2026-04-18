using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtOnline.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ArtworkLikeController(ILogger logger, IUserService userService, IArtworkLikeService likeService)
    : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Like([FromBody] Guid artworkId)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null
            ? FromServiceResponse(await likeService.LikeArtwork(artworkId, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> Unlike([FromBody] Guid artworkId)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null
            ? FromServiceResponse(await likeService.UnlikeArtwork(artworkId, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<int>>> GetLikes([FromQuery] Guid artworkId)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await likeService.GetLikesCount(artworkId))
            : ErrorMessageResult<int>(currentUser.Error);
    }
}