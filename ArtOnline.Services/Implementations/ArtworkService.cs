using System.Net;
using ArtOnline.Database.Repository;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Database.Repository.Enums;
using ArtOnline.Infrastructure.DataTransferObjects;
using ArtOnline.Infrastructure.Errors;
using ArtOnline.Infrastructure.Repositories.Interfaces;
using ArtOnline.Infrastructure.Requests;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.DataTransferObjects;
using ArtOnline.Services.DataTransferObjects.Artwork;
using ArtOnline.Services.DataTransferObjects.User;
using ArtOnline.Services.Specifications;

namespace ArtOnline.Services.Implementations;

public class ArtworkService(IRepository<WebAppDatabaseContext> repository, IFileRepository fileRepository) : IArtworkService
{
    private static string GetImageDirectory(Guid userId) => Path.Join(userId.ToString(), IUserFileService.UserFilesDirectory);
    
    public async Task<ServiceResponse<ArtworkRecord>> GetArtwork(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAsync(new ArtworkProjectionSpec(id), cancellationToken);

        return result != null
            ? ServiceResponse.ForSuccess(result)
            : ServiceResponse.FromError<ArtworkRecord>(new(HttpStatusCode.NotFound, "Artwork not found!", ErrorCodes.EntityNotFound));
    }

    public async Task<ServiceResponse<PagedResponse<ArtworkRecord>>> GetArtworks(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new ArtworkProjectionSpec(pagination.Search), cancellationToken);

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<PagedResponse<ArtworkRecord>>> GetArtworksByArtistProfile(Guid artistProfileId, PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await repository.PageAsync(pagination, new ArtworkProjectionSpec(pagination.Search, artistProfileId), cancellationToken);

        return ServiceResponse.ForSuccess(result);
    }

    public async Task<ServiceResponse<FileRecord>> GetArtworkImage(Guid id, CancellationToken cancellationToken = default)
    {
        var artwork = await repository.GetAsync(new ArtworkProjectionSpec(id), cancellationToken);
        
        return artwork != null
            ? fileRepository.GetFile(artwork.ImageUrl)
            : ServiceResponse.FromError<FileRecord>(new(HttpStatusCode.NotFound, "Artwork not found!", ErrorCodes.EntityNotFound));
    }

    public async Task<ServiceResponse> AddArtwork(ArtworkAddRecord artwork, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Artist)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only artists can add artworks!", ErrorCodes.CannotAdd));
        }
        
        var artistProfile = await repository.GetAsync(new ArtistProfileSpec(artwork.ArtistProfileId, true), cancellationToken);

        if (artistProfile == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artist profile not found!", ErrorCodes.EntityNotFound));
        }

        if (requestingUser != null && requestingUser.Id != artistProfile.UserId && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only add artworks to your own artist profile!", ErrorCodes.CannotAdd));
        }
        
        var savedFile = fileRepository.SaveFile(artwork.ImageFile, GetImageDirectory(artwork.ArtistProfileId));

        if (savedFile.Result == null)
        {
            return savedFile.ToResponse();
        }

        await repository.AddAsync(new Artwork
        {
            Title = artwork.Title,
            Description = artwork.Description,
            ImageUrl = Path.Join(GetImageDirectory(artwork.ArtistProfileId), savedFile.Result),
            ArtistProfileId = artwork.ArtistProfileId,
            Views = 0,
            Shares = 0
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
    
    public async Task<ServiceResponse> UpdateArtwork(ArtworkUpdateRecord artwork, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Artist)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only artists can updated artworks!", ErrorCodes.CannotUpdate));
        }
        
        var artistProfile = await repository.GetAsync(new ArtistProfileSpec(artwork.ArtistProfileId, true), cancellationToken);

        if (artistProfile == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artist profile not found!", ErrorCodes.EntityNotFound));
        }

        if (requestingUser != null && requestingUser.Id != artistProfile.UserId && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only update artworks to your own artist profile!", ErrorCodes.CannotAdd));
        }

        var entity = await repository.GetAsync(new ArtworkSpec(artwork.Id), cancellationToken);

        if (entity != null)
        {
            entity.Title = artwork.Title ?? entity.Title;
            entity.Description = artwork.Description ?? entity.Description;

            if (artwork.ImageFile != null)
            {
                fileRepository.DeleteFile(entity.ImageUrl);
                var savedFile = fileRepository.SaveFile(artwork.ImageFile, GetImageDirectory(artwork.ArtistProfileId));
                entity.ImageUrl = Path.Join(GetImageDirectory(artwork.ArtistProfileId), savedFile.Result);
            }
            
            await repository.UpdateAsync(entity, cancellationToken);
        }
        
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteArtwork(Guid id, UserRecord? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser != null && requestingUser.Role != UserRoleEnum.Artist)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "Only artists can updated artworks!", ErrorCodes.CannotAdd));
        }
        
        var artwork = await repository.GetAsync(new ArtworkSpec(id), cancellationToken);
        
        if (artwork == null)
        {
            return ServiceResponse.ForSuccess();
        }
        
        var artistProfile = await repository.GetAsync(new ArtistProfileSpec(artwork.ArtistProfileId, true), cancellationToken);

        if (artistProfile == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artist profile not found!", ErrorCodes.EntityNotFound));
        }

        if (requestingUser != null && requestingUser.Id != artistProfile.UserId && requestingUser.Role != UserRoleEnum.Admin)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.Forbidden, "You can only delete artworks to your own artist profile!", ErrorCodes.CannotAdd));
        }
        
        fileRepository.DeleteFile(artwork.ImageUrl);
        await repository.DeleteAsync<Artwork>(id, cancellationToken);
        
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> IncrementViews(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new ArtworkSpec(id), cancellationToken);

        if (entity == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artwork not found!", ErrorCodes.EntityNotFound));
        }

        entity.Views++;
        await repository.UpdateAsync(entity, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> IncrementShares(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetAsync(new ArtworkSpec(id), cancellationToken);

        if (entity == null)
        {
            return ServiceResponse.FromError(new(HttpStatusCode.NotFound, "Artwork not found!", ErrorCodes.EntityNotFound));
        }

        entity.Shares++;
        await repository.UpdateAsync(entity, cancellationToken);

        return ServiceResponse.ForSuccess();
    }
}