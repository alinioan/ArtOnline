using ArtOnline.Infrastructure.BaseObjects;

namespace ArtOnline.Database.Repository.Entities;

public class Collection : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsPrivate { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<CollectionArtwork> CollectionArtworks { get; set; } = null!;
}
