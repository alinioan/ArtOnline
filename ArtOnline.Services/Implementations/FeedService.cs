using ArtOnline.Database.Repository;
using ArtOnline.Database.Repository.Enums;
using ArtOnline.Infrastructure.Repositories.Interfaces;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.DataTransferObjects.Artwork;
using ArtOnline.Services.Specifications;

namespace ArtOnline.Services.Implementations;

public class FeedService(IRepository<WebAppDatabaseContext> repository) : IFeedService
{
    public async Task<ServiceResponse<PagedResponse<ArtworkRecord>>> GetArtworkFeed(
        PaginationSearchQueryParams pagination,
        ArtworkOrderEnum order,
        CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new ArtworkFeedSpec(order), cancellationToken);

        return ServiceResponse.ForSuccess(result);
    }
}