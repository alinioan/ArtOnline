namespace ArtOnline.Services.DataTransferObjects.ArtistProfile;

public class ArtistProfileAddRecord
{
    public Guid Id { get; set; }
    public String? Bio { get; set; }
    
    public Guid UserId { get; set; }
}