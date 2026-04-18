using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.DataTransferObjects;
using ArtOnline.Services.DataTransferObjects.User;

namespace ArtOnline.Services.Abstractions;

public interface ITagService
{
    Task<ServiceResponse<TagRecord>> GetTag(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResponse<PagedResponse<TagRecord>>> GetTags(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddTag(TagRecord tag, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateTag(TagRecord tag, UserRecord? requestingUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteTag(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default);
}