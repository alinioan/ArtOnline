namespace ArtOnline.Services.DataTransferObjects.ArtistProfile;

public class ArtistProfileRecord
{
    public Guid Id { get; set; }
    public String? Bio { get; set; }
    
    public Guid UserId { get; set; }
    public List<Guid>? ArtworkIds { get; set; }
}