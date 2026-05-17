using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.DataTransferObjects.Feedback;

namespace ArtOnline.Services.Abstractions;

public interface IFeedbackFormService
{
    public Task<ServiceResponse> SubmitFeedback(FeedbackSubmitRecord feedback, CancellationToken cancellationToken = default);
}