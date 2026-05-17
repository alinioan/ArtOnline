namespace ArtOnline.Services.DataTransferObjects.Feedback;

public record FeedbackSubmitRecord(
    IList<FeedbackOptionRecord> Options,
    string Message,
    string? ContactReason,
    string? ContactEmail
);

public record FeedbackOptionRecord(
    string Category,
    string SatisfactionLevel
);