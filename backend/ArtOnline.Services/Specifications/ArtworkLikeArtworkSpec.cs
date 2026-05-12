using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;

namespace ArtOnline.Services.Specifications;

public class ArtworkLikeArtworkSpec : Specification<ArtworkLike>
{ 
    public ArtworkLikeArtworkSpec(Guid artworkId) => Query.Where(al => al.ArtworkId == artworkId);
}