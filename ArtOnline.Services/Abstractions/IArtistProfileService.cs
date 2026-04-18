using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.DataTransferObjects.ArtistProfile;
using ArtOnline.Services.DataTransferObjects.User;

namespace ArtOnline.Services.Abstractions;

public interface IArtistProfileService
{
    Task<ServiceResponse<ArtistProfileRecord>> GetArtistProfile(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<ArtistProfileRecord>> GetArtistProfileByUserId(Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<ArtistProfileRecord>>> GetArtistProfiles(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddArtistProfile(ArtistProfileAddRecord artistProfile, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateArtistProfile(ArtistProfileUpdateRecord artistProfile, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteArtistProfile(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
}