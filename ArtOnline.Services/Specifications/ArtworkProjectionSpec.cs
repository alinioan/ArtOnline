using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Services.DataTransferObjects;
using ArtOnline.Services.DataTransferObjects.Artwork;
using Microsoft.EntityFrameworkCore;

namespace ArtOnline.Services.Specifications;


public class ArtworkProjectionSpec : Specification<Artwork, ArtworkRecord>
{
    public ArtworkProjectionSpec(Guid id) => Query.Where(a => a.Id == id);

    public ArtworkProjectionSpec(string? search)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(a => EF.Functions.ILike(a.Title, searchExpr));
    }
    
    public ArtworkProjectionSpec(string? search, Guid? artistProfileId)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;
        
    }
}