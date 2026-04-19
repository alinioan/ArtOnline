using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Services.DataTransferObjects;
using ArtOnline.Services.DataTransferObjects.Artwork;
using Microsoft.EntityFrameworkCore;

namespace ArtOnline.Services.Specifications;


public class ArtworkProjectionSpec : Specification<Artwork, ArtworkRecord>
{
    public ArtworkProjectionSpec(bool orderByCreatedAt = false) =>
        Query.OrderByDescending(a => a.CreatedAt, orderByCreatedAt)
            .Select(a => new()
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                ImageUrl = a.ImageUrl,
                Shares = a.Shares,
                Views = a.Views,
                ArtistProfileId = a.ArtistProfileId,
                TagIds = a.ArtworkTags.Select(at => at.TagId).ToList(),
                CollectionIds = a.CollectionArtworks.Select(ca => ca.CollectionId).ToList()
            });
    
    public ArtworkProjectionSpec(Guid id) : this() => Query.Where(a => a.Id == id);

    public ArtworkProjectionSpec(string? search) : this(true)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(a => EF.Functions.ILike(a.Title, searchExpr));
    }
    
    public ArtworkProjectionSpec(string? search, Guid? artistProfileId) : this(search)
    {
        Query.Where(a => a.ArtistProfileId == artistProfileId);
    }
}