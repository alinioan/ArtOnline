using ArtOnline.Database.Repository.Enums;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.DataTransferObjects.Artwork;

namespace ArtOnline.Services.Abstractions;

public interface IFeedService
{
    Task<ServiceResponse<PagedResponse<ArtworkRecord>>> GetArtworkFeed(
        PaginationSearchQueryParams pagination, 
        ArtworkOrderEnum order, 
        CancellationToken cancellationToken = default);
}