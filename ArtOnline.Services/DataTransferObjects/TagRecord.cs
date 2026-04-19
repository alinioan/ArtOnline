namespace ArtOnline.Services.DataTransferObjects;

public class TagRecord
{
    public Guid Id { get; set; }
    public String Name { get; set; } = null!;
    public List<Guid>? ArtworkIds { get; set; }
}