namespace ArtOnline.Services.DataTransferObjects.Collection;

public class CollectionRecord
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsPrivate { get; set; }
    
    public Guid UserId { get; set; }
}