using Application.Helper;
using Application.ResponseMessage;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;


namespace Application.UseCases.Feedbacks.Commands.CreateFeedback;

public class CreateFeedbackHandler : IRequestHandler<CreateFeedbackCommand, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    public CreateFeedbackHandler(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetUserByIdAsync(request.UserId);
        if (userEntity == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageUser.UserNotFound,
                Data = null
            };
        }
        Feedback newFeedback = new Feedback(request.UserId, request.Feedback.EventId, request.Feedback.Content, request.Feedback.Rating);

        newFeedback.CreatedAt = DateTimeHelper.GetDateTimeNow();
        await _feedbackRepository.Add(newFeedback);
        if (await _unitOfWork.SaveChangesAsync() > 0)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.Created,
                Message = MessageCommon.SavingSuccesfully,
                Data = request.Feedback
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.SavingFailed,
            Data = null
        };
    }
}
