using ArtOnline.Infrastructure.Handlers;
using ArtOnline.Infrastructure.Responses;
using ArtOnline.Services.Abstractions;
using ArtOnline.Services.DataTransferObjects.Feedback;
using Microsoft.AspNetCore.Mvc;

namespace ArtOnline.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class FeedbackFormController(ILogger<FeedbackFormController> logger, IFeedbackFormService feedbackFormService)
    : BaseResponseController(logger)
{
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Submit([FromBody] FeedbackSubmitRecord feedback, CancellationToken cancellationToken)
    {
        return FromServiceResponse(await feedbackFormService.SubmitFeedback(feedback, cancellationToken));
    }
}
