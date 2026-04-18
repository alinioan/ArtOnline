using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;

namespace ArtOnline.Services.Specifications;

public class ArtworkLikeSpec : Specification<ArtworkLike>
{
    public ArtworkLikeSpec(Guid id) => Query.Where(al => al.ArtworkId == id);
    
    public ArtworkLikeSpec(Guid userId, Guid artworkId) => Query.Where(al => al.UserId == userId && al.ArtworkId == artworkId); 
}