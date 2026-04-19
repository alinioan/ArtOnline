using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Services.DataTransferObjects;

namespace ArtOnline.Services.Specifications;

public class TagProjectionSpec : Specification<Tag, TagRecord>
{
    public TagProjectionSpec(bool orderByCreatedAt = false) =>
        Query.OrderByDescending(t => t.CreatedAt, orderByCreatedAt)
            .Select(t => new()
            {
                Id = t.Id,
                Name = t.Name,
                ArtworkIds = t.ArtworkTags.Select(at => at.ArtworkId).ToList()
            });
    
    public TagProjectionSpec(Guid id) : this() => Query.Where(t => t.Id == id);
    
    public TagProjectionSpec(string? name) : this() => Query.Where(t => t.Name == name);
}
