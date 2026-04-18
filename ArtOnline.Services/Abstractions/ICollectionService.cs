using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.DataTransferObjects.Collection;
using ArtOnline.Services.DataTransferObjects.User;

namespace ArtOnline.Services.Abstractions;

public interface ICollectionService
{
    Task<ServiceResponse<CollectionRecord>> GetCollection(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<CollectionRecord>>> GetCollections(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<CollectionRecord>>> GetCollectionsByUser(Guid userId, PaginationSearchQueryParams pagination, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddCollection(CollectionAddRecord collection, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateCollection(CollectionUpdateRecord collection, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteCollection(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddArtworkToCollection(Guid collectionId, Guid artworkId, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> RemoveArtworkFromCollection(Guid collectionId, Guid artworkId, UserRecord? requestingUser, CancellationToken cancellationToken = default);
}