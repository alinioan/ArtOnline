using ArtOnline.Infrastructure.BaseObjects;

namespace ArtOnline.Database.Repository.Entities;

public class TagConfiguration : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<Artwork> Artworks { get; set; } = null!;
}