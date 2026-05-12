using System.Net.Mime;
using ArtOnline.Infrastructure.DataTransferObjects;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.Authorization;
using ArtOnline.Services.DataTransferObjects.ArtistProfile;
using ArtOnline.Services.DataTransferObjects.Artwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtOnline.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ArtistProfileController(ILogger<ArtistProfileController> logger, IUserService userService, IArtistProfileService artistProfileService)
    : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ArtistProfileRecord>>> GetById([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artistProfileService.GetArtistProfile(id))
            : ErrorMessageResult<ArtistProfileRecord>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ArtistProfileRecord>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artistProfileService.GetArtistProfiles(pagination))
            : ErrorMessageResult<PagedResponse<ArtistProfileRecord>>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<RequestResponse<ArtistProfileRecord>>> GetByUserId(
        [FromRoute] Guid userId,
        [FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artistProfileService.GetArtistProfileByUserId(userId))
            : ErrorMessageResult<ArtistProfileRecord>(currentUser.Error);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ArtistProfileAddRecord artistProfile)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artistProfileService.AddArtistProfile(artistProfile, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] ArtistProfileUpdateRecord artistProfile)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artistProfileService.UpdateArtistProfile(artistProfile, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artistProfileService.DeleteArtistProfile(id, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }
}