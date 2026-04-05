using ArtOnline.Database.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtOnline.Database.Repository.EntityConfigurations;

public class ArtworkLikeConfiguration : IEntityTypeConfiguration<ArtworkLike>
{
    public void Configure(EntityTypeBuilder<ArtworkLike> builder)
    {
        builder.HasKey(al => al.Id);

        builder.HasOne(al => al.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(al => al.UserId);

        builder.HasOne(al => al.Artwork)
            .WithMany(a => a.Likes)
            .HasForeignKey(al => al.ArtworkId);
        
        builder.HasIndex(al => new { al.UserId, al.ArtworkId })
            .IsUnique();
    }
}