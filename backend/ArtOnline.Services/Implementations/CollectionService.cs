using System.Net;
using ArtOnline.Database.Repository;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Database.Repository.Enums;
using ArtOnline.Infrastructure.Errors;
using ArtOnline.Infrastructure.Repositories.Interfaces;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.DataTransferObjects.Collection;
using ArtOnline.Services.DataTransferObjects.User;
using ArtOnline.Services.Specifications;

namespace ArtOnline.Services.Implementations;

public class CollectionService(IRepository<WebAppDatabaseContext> repository) : ICollectionService
{
    public async Task<ServiceResponse<CollectionRecord>> GetCollection(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new CollectionProjectionSpec(id), cancellationToken);

        if (result == null)
        {
            return ServiceResponse.FromError<CollectionRecord>(new(HttpStatusCode.NotFound, "Collection not found!", ErrorCodes.EntityNotFound));
        }

        if (result.IsPrivate && requestingUser?.Id != result.UserId && requestingUser?.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError<CollectionRecord>(new(HttpStatusCode.Forbidden, "This collection is private!", ErrorCodes.AccessDenied));
        }

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<CollectionRecord>>> GetCollections(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new CollectionProjectionSpec(pagination.Search, false), cancellationToken);

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<CollectionRecord>>> GetCollectionsByUser(Guid userId, PaginationSearchQueryParams pagination, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var includePrivate = requestingUser?.Id == userId || requestingUser?.Role == UserRoleEnum.Admin;

        var result = await repository.PageAsync(pagination, new CollectionProjectionSpec(pagination.Search, includePrivate, userId), cancellationToken);

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> AddCollection(CollectionAddRecord collection, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Id != collection.UserId && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only create your own collections!", ErrorCodes.CannotAdd));
        }

        await repository.AddAsync(new Collection
        {
            Name = collection.Name,
            Description = collection.Description,
            IsPrivate = collection.IsPrivate,
            UserId = collection.UserId
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateCollection(CollectionUpdateRecord collection, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new CollectionSpec(collection.Id), cancellationToken);

        if (entity == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Collection not found!", ErrorCodes.EntityNotFound));
        }

        if (requestingUser != null && requestingUser.Id != entity.UserId && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only update your own collections!", ErrorCodes.CannotUpdate));
        }

        entity.Name = collection.Name ?? entity.Name;
        entity.Description = collection.Description ?? entity.Description;
        entity.IsPrivate = collection.IsPrivate;

        await repository.UpdateAsync(entity, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteCollection(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new CollectionSpec(id), cancellationToken);

        if (entity == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Collection not found!", ErrorCodes.EntityNotFound));
        }

        if (requestingUser != null && requestingUser.Id != entity.UserId && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only delete your own collections!", ErrorCodes.CannotDelete));
        }

        await repository.DeleteAsync<Collection>(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> AddArtworkToCollection(CollectionArtworkRecord collectionArtwork, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var collectionId = collectionArtwork.CollectionId;
        var artworkId = collectionArtwork.ArtworkId;
        var collection = await repository.GetAsync(new CollectionSpec(collectionId), cancellationToken);

        if (collection == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Collection not found!", ErrorCodes.EntityNotFound));
        }

        if (requestingUser != null && requestingUser.Id != collection.UserId && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only modify your own collections!", ErrorCodes.CannotUpdate));
        }

        var artwork = await repository.GetAsync(new ArtworkSpec(artworkId), cancellationToken);

        if (artwork == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artwork not found!", ErrorCodes.EntityNotFound));
        }

        var existing = await repository.GetAsync(new CollectionArtworkSpec(collectionId, artworkId), cancellationToken);

        if (existing != null)   
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "Artwork already in collection!", ErrorCodes.EntityAlreadyExists));
        }

        await repository.AddAsync(new CollectionArtwork
        {
            CollectionId = collectionId,
            ArtworkId = artworkId
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> RemoveArtworkFromCollection(CollectionArtworkRecord collectionArtworkIds, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var collectionId = collectionArtworkIds.CollectionId;
        var artworkId = collectionArtworkIds.ArtworkId;
        var collection = await repository.GetAsync(new CollectionSpec(collectionId), cancellationToken);

        if (collection == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Collection not found!", ErrorCodes.EntityNotFound));
        }

        if (requestingUser != null && requestingUser.Id != collection.UserId && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only modify your own collections!", ErrorCodes.CannotUpdate));
        }

        var collectionArtwork = await repository.GetAsync(new CollectionArtworkSpec(collectionId, artworkId), cancellationToken);

        if (collectionArtwork == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artwork not in collection!", ErrorCodes.EntityNotFound));
        }

        await repository.DeleteAsync<CollectionArtwork>(collectionArtwork.Id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}