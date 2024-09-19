using Domain.DTOs.Feedbacks;
using Domain.Models.Response;
using MediatR;


namespace Application.UseCases.Feedbacks.Commands.CreateFeedback;

public class CreateFeedbackCommand: IRequest<APIResponse>
{
    public FeedbackDto Feedback { get; set; }
    public Guid UserId { get; set; }    
    public CreateFeedbackCommand(FeedbackDto feedback, Guid userId)
    {
        Feedback = feedback;
        UserId = userId;
    }
}
