using ArtOnline.Database.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ArtworkConfiguration : IEntityTypeConfiguration<Artwork>
{
    public void Configure(EntityTypeBuilder<Artwork> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(a => a.Description)
            .HasMaxLength(1000);
        builder.Property(a => a.ImageUrl)
            .IsRequired();
        builder.Property(a => a.Views)
            .HasDefaultValue(0);
        builder.Property(a => a.Shares)
            .HasDefaultValue(0);
        
        builder.HasOne(a => a.ArtistProfile)
            .WithMany(ap => ap.Artworks)
            .HasForeignKey(a => a.ArtistProfileId);
    }
}