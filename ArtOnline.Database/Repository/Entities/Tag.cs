using ArtOnline.Infrastructure.BaseObjects;

namespace ArtOnline.Database.Repository.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<ArtworkTag> ArtworkTags { get; set; } = null!;
}