using ArtOnline.Infrastructure.BaseObjects;

namespace ArtOnline.Database.Repository.Entities;

public class ArtworkTag : BaseEntity
{
    public Guid ArtworkId { get; set; }
    public Artwork Artwork { get; set; } = null!;
    
    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}