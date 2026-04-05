using ArtOnline.Database.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtOnline.Database.Repository.EntityConfigurations;

public class ArtworkTagConfiguration : IEntityTypeConfiguration<ArtworkTag>
{
    public void Configure(EntityTypeBuilder<ArtworkTag> builder)
    {
        builder.HasKey(at => new { at.ArtworkId, at.TagId });

        builder.HasOne(at => at.Artwork)
            .WithMany(a => a.ArtworkTags)
            .HasForeignKey(at => at.ArtworkId);

        builder.HasOne(at => at.Tag)
            .WithMany(t => t.ArtworkTags)
            .HasForeignKey(at => at.TagId);
    }
}