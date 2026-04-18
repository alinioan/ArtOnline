using System.Net;
using ArtOnline.Database.Repository;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Database.Repository.Enums;
using ArtOnline.Infrastructure.Errors;
using ArtOnline.Infrastructure.Repositories.Interfaces;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.DataTransferObjects;
using ArtOnline.Services.DataTransferObjects.User;
using ArtOnline.Services.Specifications;

namespace ArtOnline.Services.Implementations;

public class TagService(IRepository<WebAppDatabaseContext> repository) : ITagService
{
    public async Task<ServiceResponse<TagRecord>> GetTag(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new TagProjectionSpec(id), cancellationToken);

        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<TagRecord>(new(HttpStatusCode.NotFound, "Tag not found!", ErrorCodes.EntityNotFound));
    }

    public async Task<ServiceResponse<PagedResponse<TagRecord>>> GetTags(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new TagProjectionSpec(pagination.Search), cancellationToken);

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> AddTag(TagRecord tag, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only admins can add tags!", ErrorCodes.CannotAdd));
        }

        var existing = await repository.GetAsync(new TagSpec(tag.Name), cancellationToken);

        if (existing != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "Tag already exists!", ErrorCodes.EntityAlreadyExists));
        }

        await repository.AddAsync(new Tag
        {
            Name = tag.Name
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateTag(TagRecord tag, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only admins can update tags!", ErrorCodes.CannotUpdate));
        }

        var entity = await repository.GetAsync(new TagSpec(tag.Id), cancellationToken);

        if (entity == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Tag not found!", ErrorCodes.EntityNotFound));
        }

        entity.Name = tag.Name ?? entity.Name;

        await repository.UpdateAsync(entity, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteTag(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only admins can delete tags!", ErrorCodes.CannotDelete));
        }

        await repository.DeleteAsync<Tag>(id, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}