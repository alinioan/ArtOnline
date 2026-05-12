namespace ArtOnline.Services.DataTransferObjects.Collection;

public class CollectionAddRecord
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsPrivate { get; set; }
    
    public Guid UserId { get; set; }
}