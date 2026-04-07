using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;

namespace ArtOnline.Services.Specifications;

public class ArtistProfileSpec : Specification<ArtistProfile>
{
    public ArtistProfileSpec(Guid artistId) => Query.Where(a => a.Id == artistId);
}