using ArtOnline.Infrastructure.BaseObjects;

using ArtOnline.Database.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class ArtistProfileConfiguration : IEntityTypeConfiguration<ArtistProfile>
{
    public void Configure(EntityTypeBuilder<ArtistProfile> builder)
    {
        builder.HasKey(ap => ap.Id);

        builder.Property(ap => ap.Bio)
            .HasMaxLength(500);

        builder.HasOne(ap => ap.User)
            .WithOne(u => u.ArtistProfile)
            .HasForeignKey<ArtistProfile>(ap => ap.UserId);

        builder.HasMany(ap => ap.Artworks)
            .WithOne(a => a.ArtistProfile)
            .HasForeignKey(a => a.ArtistProfileId);
    }
        
}