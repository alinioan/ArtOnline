using Ardalis.Specification;
using ArtOnline.Database.Repository.Entities;
using ArtOnline.Database.Repository.Enums;
using ArtOnline.Services.DataTransferObjects.Artwork;

namespace ArtOnline.Services.Specifications;

public class ArtworkFeedSpec : Specification<Artwork, ArtworkRecord>
{
    public ArtworkFeedSpec(ArtworkOrderEnum order)
    {
        Query.Select(a => new ArtworkRecord
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            ImageUrl = a.ImageUrl,
            Views = a.Views,
            Shares = a.Shares,
            ArtistProfileId = a.ArtistProfileId,
            TagIds = a.ArtworkTags.Select(at => at.TagId).ToList(),
            Likes = a.Likes.Count
        });

        switch (order)
        {
            case ArtworkOrderEnum.Newest:
                Query.OrderByDescending(a => a.CreatedAt); 
                break;
            case ArtworkOrderEnum.Oldest:
                Query.OrderBy(a => a.CreatedAt);
                break;
            case ArtworkOrderEnum.MostLiked:
                Query.OrderByDescending(a => a.Likes.Count);
                break;
            case ArtworkOrderEnum.MostViewed:
                Query.OrderByDescending(a => a.Views);
                break;
            case ArtworkOrderEnum.Random:
                Query.OrderBy(a => Guid.NewGuid());
                break;
        }
    }
}