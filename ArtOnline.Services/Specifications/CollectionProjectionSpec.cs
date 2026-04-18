using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Services.DataTransferObjects.Collection;
using Microsoft.EntityFrameworkCore;

namespace ArtOnline.Services.Specifications;

public class CollectionProjectionSpec : Specification<Collection, CollectionRecord>
{
    public CollectionProjectionSpec(bool orderByCreatedAt = false) =>
        Query.OrderByDescending(c => c.CreatedAt, orderByCreatedAt)
            .Select(c => new()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsPrivate = c.IsPrivate,
                UserId = c.UserId,
            });
    
    public CollectionProjectionSpec(Guid id) : this() => Query.Where(c => c.Id == id);
    
    public CollectionProjectionSpec(Guid id, bool isUserId) : this() => Query.Where(c => c.UserId == id);
    
    public CollectionProjectionSpec(string? search) : this(true)
    {
        search = !string.IsNullOrWhiteSpace(search) ? search.Trim() : null;

        if (search == null)
        {
            return;
        }

        var searchExpr = $"%{search.Replace(" ", "%")}%";

        Query.Where(e => e.IsPrivate == false &&
                         (EF.Functions.ILike(e.Name, searchExpr) ||
                         EF.Functions.ILike(e.Description!, searchExpr)));
        
        
    }
}