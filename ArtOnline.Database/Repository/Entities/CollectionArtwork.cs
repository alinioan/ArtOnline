using ArtOnline.Infrastructure.BaseObjects;

namespace ArtOnline.Database.Repository.Entities;

public class CollectionArtwork : BaseEntity
{
    public Guid CollectionId { get; set; }
    public Collection Collection { get; set; } = null!;

    public Guid ArtworkId { get; set; }
    public Artwork Artwork { get; set; } = null!;
}