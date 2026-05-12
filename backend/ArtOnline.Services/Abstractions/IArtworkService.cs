using ArtOnline.Infrastructure.DataTransferObjects;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.DataTransferObjects;
using ArtOnline.Services.DataTransferObjects.Artwork;
using ArtOnline.Services.DataTransferObjects.User;

namespace ArtOnline.Services.Abstractions;

public interface IArtworkService
{
    Task<ServiceResponse<ArtworkRecord>> GetArtwork(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<ArtworkRecord>>> GetArtworks(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<ArtworkRecord>>> GetArtworksByArtistProfile(Guid artistProfileId, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse<FileRecord>> GetArtworkImage(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddArtwork(ArtworkAddRecord artwork, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateArtwork(ArtworkUpdateRecord artwork, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteArtwork(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> IncrementViews(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse> IncrementShares(Guid id, CancellationToken cancellationToken = default);
}