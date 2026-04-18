using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;

namespace ArtOnline.Services.Specifications;

public class CollectionArtworkSpec : Specification<CollectionArtwork>
{
    public CollectionArtworkSpec(Guid id) => Query.Where(ca => ca.CollectionId == id);
    
    public CollectionArtworkSpec(Guid collectionId, Guid artworkId) => Query.Where(ca => ca.CollectionId == collectionId && ca.ArtworkId == artworkId);  
}