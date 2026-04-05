using ArtOnline.Database.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtOnline.Database.Repository.EntityConfigurations;

public class CollectionArtworkConfiguration : IEntityTypeConfiguration<CollectionArtwork>
{
    public void Configure(EntityTypeBuilder<CollectionArtwork> builder)
    {
        builder.HasKey(ca => new { ca.CollectionId, ca.ArtworkId });

        builder.HasOne(ca => ca.Collection)
            .WithMany(c => c.CollectionArtworks)
            .HasForeignKey(ca => ca.CollectionId);

        builder.HasOne(ca => ca.Artwork)
            .WithMany(a => a.CollectionArtworks)
            .HasForeignKey(ca => ca.ArtworkId);

        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired();
    }
}