using System.Net;
using ArtOnline.Database.Repository;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Infrastructure.Errors;
using ArtOnline.Infrastructure.Repositories.Interfaces;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.DataTransferObjects.User;
using ArtOnline.Services.Specifications;

namespace ArtOnline.Services.Implementations;

public class ArtworkLikeService(IRepository<WebAppDatabaseContext> repository) : IArtworkLikeService
{
    public async Task<ServiceResponse<int>> GetLikesCount(Guid artworkId, CancellationToken cancellationToken = default)
    {
        var count = await repository.GetCountAsync(new ArtworkLikeArtworkSpec(artworkId), cancellationToken);

        return ServiceResponse.ForSuccess(count);
    }

    public async Task<ServiceResponse> LikeArtwork(Guid artworkId, UserRecord requestingUser, CancellationToken cancellationToken = default)
    {
        var artwork = await repository.GetAsync(new ArtworkSpec(artworkId), cancellationToken);

        if (artwork == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artwork not found!", ErrorCodes.EntityNotFound));
        }

        var existing = await repository.GetAsync(new ArtworkLikeSpec(requestingUser.Id, artworkId), cancellationToken);

        if (existing != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "You already liked this artwork!", ErrorCodes.EntityAlreadyExists));
        }

        await repository.AddAsync(new ArtworkLike
        {
            ArtworkId = artworkId,
            UserId = requestingUser.Id
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UnlikeArtwork(Guid artworkId, UserRecord requestingUser, CancellationToken cancellationToken = default)
    {
        var like = await repository.GetAsync(new ArtworkLikeSpec(requestingUser.Id, artworkId), cancellationToken);

        if (like == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "You haven't liked this artwork!", ErrorCodes.EntityNotFound));
        }

        await repository.DeleteAsync<ArtworkLike>(like.Id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}