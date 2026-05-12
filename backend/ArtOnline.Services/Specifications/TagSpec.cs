using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;

namespace ArtOnline.Services.Specifications;

public class TagSpec : Specification<Tag>
{
    public TagSpec(Guid id) => Query.Where(t => t.Id == id);
    
    public TagSpec(string name) => Query.Where(t => t.Name == name); 
}