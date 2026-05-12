using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.Authorization;
using ArtOnline.Services.DataTransferObjects;
using ArtOnline.Services.DataTransferObjects.Artwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtOnline.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TagController(ILogger logger, IUserService userService, ITagService tagService)
    : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] TagAddRecord tag)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null
            ? FromServiceResponse(await tagService.AddTag(tag, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> Delete([FromBody] Guid tagId)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null
            ? FromServiceResponse(await tagService.DeleteTag(tagId, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] TagRecord tag)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await tagService.UpdateTag(tag, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<TagRecord>>> Get(Guid id)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null
            ? FromServiceResponse(await tagService.GetTag(id))
            : ErrorMessageResult<TagRecord>(currentUser.Error);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<TagRecord>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await tagService.GetTags(pagination))
            : ErrorMessageResult<PagedResponse<TagRecord>>(currentUser.Error);
    }
}