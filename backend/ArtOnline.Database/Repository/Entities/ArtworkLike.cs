using ArtOnline.Infrastructure.BaseObjects;

namespace ArtOnline.Database.Repository.Entities;

public class ArtworkLike : BaseEntity
{
    public Guid ArtworkId { get; set; }
    public Artwork Artwork { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}