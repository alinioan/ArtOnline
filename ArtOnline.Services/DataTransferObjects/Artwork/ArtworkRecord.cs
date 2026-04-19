namespace ArtOnline.Services.DataTransferObjects.Artwork;

public class ArtworkRecord
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int Views { get; set; }
    public int Shares { get; set; }
    public Guid ArtistProfileId { get; set; }
    public List<Guid>? TagIds { get; set; }
    public List<Guid>? CollectionIds { get; set; }
}