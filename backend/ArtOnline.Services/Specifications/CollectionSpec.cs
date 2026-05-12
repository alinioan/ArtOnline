using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;

namespace ArtOnline.Services.Specifications;

public class CollectionSpec : Specification<Collection>
{
    public CollectionSpec(Guid id) => Query.Where(c => c.Id == id);
}