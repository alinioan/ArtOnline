using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArtOnline.Services.Specifications;

public class ArtistProfileSpec : Specification<ArtistProfile>
{
    public ArtistProfileSpec(Guid id, bool ownId)
    {
        if (ownId)
        {
            Query.Where(a => a.Id == id);
        }
        else
        {
            Query.Where(a => a.UserId == id);       
        }
    }
}