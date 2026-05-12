using ArtOnline.Infrastructure.BaseObjects;

namespace ArtOnline.Database.Repository.Entities;

public class ArtistProfile : BaseEntity
{
    public String Bio { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Artwork> Artworks { get; set; } = null!;
}