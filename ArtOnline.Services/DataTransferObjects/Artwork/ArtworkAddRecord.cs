using Microsoft.AspNetCore.Http;

namespace ArtOnline.Services.DataTransferObjects.Artwork;

public class ArtworkAddRecord
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile ImageFile { get; set; } = null!;
    public Guid ArtistProfileId { get; set; }
}