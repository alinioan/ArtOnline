using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;

namespace ArtOnline.Services.Specifications;

public class ArtworkSpec : Specification<Artwork>
{
    public ArtworkSpec(Guid id) => Query.Where(a => a.Id == id).Include(a => a.ArtworkTags);
    
    public ArtworkSpec(string title) => Query.Where(a => a.Title == title);
}