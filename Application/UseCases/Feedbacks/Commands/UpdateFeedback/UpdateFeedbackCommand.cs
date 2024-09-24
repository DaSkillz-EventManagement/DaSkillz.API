using Domain.DTOs.Feedbacks;
using Domain.Models.Response;
using MediatR;

namespace Application.UseCases.Feedbacks.Commands.UpdateFeedback;

public class UpdateFeedbackCommand : IRequest<APIResponse>
{
    public FeedbackDto Feedback { get; set; }
    public Guid UserId { get; set; }
    public UpdateFeedbackCommand(FeedbackDto feedback, Guid userId)
    {
        Feedback = feedback;
        UserId = userId;
    }
}
