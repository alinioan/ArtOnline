using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using ArtOnline.Infrastructure.DataTransferObjects;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.Authorization;
using ArtOnline.Services.DataTransferObjects;
using ArtOnline.Services.DataTransferObjects.Artwork;

namespace ArtOnline.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ArtworkController(ILogger<ArtworkController> logger, IUserService userService, IArtworkService artworkService)
    : AuthorizedController(logger, userService)
{
    private const long MaxFileSize = 128 * 1024 * 1024;
    
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ArtworkRecord>>> GetById([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artworkService.GetArtwork(id))
            : ErrorMessageResult<ArtworkRecord>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ArtworkRecord>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artworkService.GetArtworks(pagination))
            : ErrorMessageResult<PagedResponse<ArtworkRecord>>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet("{artistProfileId:guid}")]
    public async Task<ActionResult<RequestResponse<PagedResponse<ArtworkRecord>>>> GetByArtistProfile(
        [FromRoute] Guid artistProfileId,
        [FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artworkService.GetArtworksByArtistProfile(artistProfileId, pagination))
            : ErrorMessageResult<PagedResponse<ArtworkRecord>>(currentUser.Error);
    }
    
    [Authorize]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [RequestSizeLimit(MaxFileSize)]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromForm] ArtworkAddRecord artwork)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artworkService.AddArtwork(artwork, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [RequestSizeLimit(MaxFileSize)]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromForm] ArtworkUpdateRecord artwork)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artworkService.UpdateArtwork(artwork, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artworkService.DeleteArtwork(id, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> IncrementViews([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artworkService.IncrementViews(id))
            : ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> IncrementShares([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await artworkService.IncrementShares(id))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Stream), StatusCodes.Status200OK)]
    public async Task<ActionResult<RequestResponse<FileRecord>>> GetImage([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        if (currentUser.Result == null)
        {
            return ErrorMessageResult<FileRecord>(currentUser.Error);
        }

        return FromServiceResponse(await artworkService.GetArtworkImage(id));
    }
}