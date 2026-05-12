using ArtOnline.Database.Repository.Enums;
using ArtOnline.Infrastructure.BaseObjects;

namespace ArtOnline.Database.Repository.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRoleEnum Role { get; set; }

    public ArtistProfile ArtistProfile { get; set; } = null!;
    
    public ICollection<Collection> Collections { get; set; } = null!;
    public ICollection<ArtworkLike> Likes { get; set; } = null!;
}
