using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.Authorization;
using ArtOnline.Services.DataTransferObjects.Collection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtOnline.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CollectionController(
    ILogger<ArtistProfileController> logger,
    IUserService userService,
    ICollectionService collectionService)
    : AuthorizedController(logger, userService)
{
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<CollectionRecord>>> GetById([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await collectionService.GetCollection(id, currentUser.Result))
            : ErrorMessageResult<CollectionRecord>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<CollectionRecord>>>> GetPage(
        [FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await collectionService.GetCollections(pagination))
            : ErrorMessageResult<PagedResponse<CollectionRecord>>(currentUser.Error);
    }

    [Authorize]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<RequestResponse<PagedResponse<CollectionRecord>>>> GetByUserId(
        [FromRoute] Guid userId,
        [FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await collectionService.GetCollectionsByUser(userId, pagination, currentUser.Result))
            : ErrorMessageResult<PagedResponse<CollectionRecord>>(currentUser.Error);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<CollectionRecord>>>> GetOwnCollections(
        [FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(
                await collectionService.GetCollectionsByUser(currentUser.Result.Id, pagination, currentUser.Result))
            : ErrorMessageResult<PagedResponse<CollectionRecord>>(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] CollectionAddRecord collection)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await collectionService.AddCollection(collection, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] CollectionUpdateRecord collection)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await collectionService.UpdateCollection(collection, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null
            ? FromServiceResponse(await collectionService.DeleteCollection(id, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> AddArtwork([FromBody] Guid collectionId, [FromBody] Guid artworkId)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null
            ? FromServiceResponse(await collectionService.AddArtworkToCollection(collectionId, artworkId, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> RemoveArtwork([FromBody] Guid collectionId, [FromBody] Guid artworkId)
    {
        var currentUser = await GetCurrentUser();
        
        return currentUser.Result != null
            ? FromServiceResponse(await collectionService.RemoveArtworkFromCollection(collectionId, artworkId, currentUser.Result))
            : ErrorMessageResult(currentUser.Error);
    } 
}