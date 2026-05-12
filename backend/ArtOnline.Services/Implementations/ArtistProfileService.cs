using System.Net;
using ArtOnline.Database.Repository;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Database.Repository.Enums;
using ArtOnline.Infrastructure.Errors;
using ArtOnline.Infrastructure.Repositories.Interfaces;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.DataTransferObjects.ArtistProfile;
using ArtOnline.Services.DataTransferObjects.User;
using ArtOnline.Services.Specifications;

namespace ArtOnline.Services.Implementations;

public class ArtistProfileService(IRepository<WebAppDatabaseContext> repository) : IArtistProfileService
{
    public async Task<ServiceResponse<ArtistProfileRecord>> GetArtistProfile(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new ArtistProfileProjectionSpec(id, true), cancellationToken);

        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<ArtistProfileRecord>(new(HttpStatusCode.NotFound, "Artist profile not found!", ErrorCodes.EntityNotFound));
    }

    public async Task<ServiceResponse<ArtistProfileRecord>> GetArtistProfileByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new ArtistProfileProjectionSpec(userId, false), cancellationToken);
        
        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<ArtistProfileRecord>(new(HttpStatusCode.NotFound, "Artist profile not found!", ErrorCodes.EntityNotFound));
    }

    public async Task<ServiceResponse<PagedResponse<ArtistProfileRecord>>> GetArtistProfiles(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new ArtistProfileProjectionSpec(), cancellationToken);

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse> AddArtistProfile(ArtistProfileAddRecord artistProfile, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Id != artistProfile.UserId)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only create your own artist profile!", ErrorCodes.CannotAdd));
        }
        
        var existingProfile = await repository.GetAsync(new ArtistProfileSpec(artistProfile.UserId, false), cancellationToken);

        if (existingProfile != null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Conflict, "Artist profile already exists for this user!", ErrorCodes.EntityAlreadyExists));
        }
        
        await repository.AddAsync(new ArtistProfile
        {
            Bio = artistProfile.Bio ?? "",
            UserId = artistProfile.UserId
        }, cancellationToken);
        
        var user = await repository.GetAsync(new UserSpec(artistProfile.UserId), cancellationToken);
        
        if (user != null && user.Role == UserRoleEnum.Consumer)
        {
            user.Role = UserRoleEnum.Artist;
            await repository.UpdateAsync(user, cancellationToken);    
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateArtistProfile(ArtistProfileUpdateRecord artistProfile, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Id != artistProfile.UserId)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only update your own artist profile!", ErrorCodes.CannotUpdate));
        }
        
        var entity = await repository.GetAsync(new ArtistProfileSpec(artistProfile.UserId, false), cancellationToken);
        
        if (entity == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artist profile not found!", ErrorCodes.EntityNotFound));
        }
        
        entity.Bio = artistProfile.Bio ?? entity.Bio;
        await repository.UpdateAsync(entity, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteArtistProfile(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new ArtistProfileSpec(id, true), cancellationToken);

        if (entity == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artist profile not found!", ErrorCodes.EntityNotFound));
        }

        if (requestingUser != null && requestingUser.Id != entity.UserId && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only delete your own artist profile!", ErrorCodes.CannotDelete));
        }

        await repository.DeleteAsync<ArtistProfile>(id, cancellationToken);
        
        return ServiceResponse.ForSuccess();       
    }
}