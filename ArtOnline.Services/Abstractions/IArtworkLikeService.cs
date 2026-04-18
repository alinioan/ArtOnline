using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.DataTransferObjects.User;

namespace ArtOnline.Services.Abstractions;

public interface IArtworkLikeService
{
    Task<ServiceResponse<int>> GetLikesCount(Guid artworkId, CancellationToken cancellationToken = default);
    Task<ServiceResponse<bool>> HasUserLiked(Guid artworkId, Guid userId, CancellationToken cancellationToken = default);
    Task<ServiceResponse> LikeArtwork(Guid artworkId, UserRecord requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UnlikeArtwork(Guid artworkId, UserRecord requestingUser, CancellationToken cancellationToken = default);
}