using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Services.DataTransferObjects.ArtistProfile;
using Microsoft.EntityFrameworkCore;

namespace ArtOnline.Services.Specifications;

public class ArtistProfileProjectionSpec : Specification<ArtistProfile, ArtistProfileRecord>
{
    public ArtistProfileProjectionSpec(bool orderByCreatedAt = false)
    {
        Query.OrderByDescending(a => a.CreatedAt, orderByCreatedAt)
            .Select(a => new()
            {
                Bio = a.Bio,
                UserId = a.UserId,
                Id = a.Id,
                ArtworkIds = a.Artworks.Select(art => art.Id).ToList()
            });    
    }
    
    public ArtistProfileProjectionSpec(Guid id, bool ownId) : this()
    {
        if (ownId)
        {
            Query.Where(a => a.Id == id);
        }
        else
        {
            Query.Where(a => a.UserId == id);       
        }
    }
}