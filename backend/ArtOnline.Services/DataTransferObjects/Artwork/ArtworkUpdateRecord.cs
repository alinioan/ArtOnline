using Microsoft.AspNetCore.Http;

namespace ArtOnline.Services.DataTransferObjects.Artwork;

public class ArtworkUpdateRecord
{
    public Guid Id { get; set; }
    public Guid ArtistProfileId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public IFormFile? ImageFile { get; set; }
    public List<Guid>? TagIds { get; set; }
}