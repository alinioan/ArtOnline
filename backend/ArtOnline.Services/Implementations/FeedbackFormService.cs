using System.Text;
using ArtOnline.Infrastructure.Configurations;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.DataTransferObjects.Feedback;
using Microsoft.Extensions.Options;

namespace ArtOnline.Services.Implementations;

public class FeedbackFormService(IMailService mailService, IOptions<MailConfiguration> mailConfiguration) : IFeedbackFormService
{
    private readonly MailConfiguration _mailConfiguration = mailConfiguration.Value;

    public async Task<ServiceResponse> SubmitFeedback(FeedbackSubmitRecord feedback, CancellationToken cancellationToken = default)
    {
        var bodyBuilder = new StringBuilder();
        foreach (var option in feedback.Options)
        {
            bodyBuilder.AppendLine($"Category: {option.Category}");
            bodyBuilder.AppendLine($"Satisfaction Level: {option.SatisfactionLevel}");
        }
        
        if (!string.IsNullOrWhiteSpace(feedback.ContactReason))
        {
            bodyBuilder.AppendLine($"Contact Reason: {feedback.ContactReason}");
        }

        if (!string.IsNullOrWhiteSpace(feedback.ContactEmail))
        {
            bodyBuilder.AppendLine($"Contact Email: {feedback.ContactEmail}");
        }

        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("Message:");
        bodyBuilder.AppendLine(feedback.Message);

        return await mailService.SendMail(
            _mailConfiguration.MailAddress, 
            $"Feedback Received:", 
            bodyBuilder.ToString(), 
            false, 
            "ArtOnline Feedback", 
            cancellationToken);
    }
}
