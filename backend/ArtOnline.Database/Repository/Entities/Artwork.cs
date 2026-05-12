using ArtOnline.Infrastructure.BaseObjects;

namespace ArtOnline.Database.Repository.Entities;

public class Artwork : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;

    public int Views { get; set; }
    public int Shares { get; set; }
    
    public Guid ArtistProfileId { get; set; }
    public ArtistProfile ArtistProfile { get; set; } = null!;
    
    public ICollection<ArtworkTag> ArtworkTags { get; set; } = null!;
    public ICollection<CollectionArtwork> CollectionArtworks { get; set; } = null!;
    public ICollection<ArtworkLike> Likes { get; set; } = null!;
}