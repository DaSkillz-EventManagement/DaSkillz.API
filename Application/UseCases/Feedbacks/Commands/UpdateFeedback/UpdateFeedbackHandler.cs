using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using Application.ResponseMessage;
using System.Net;
using Application.Helper;

namespace Application.UseCases.Feedbacks.Commands.UpdateFeedback;

public class UpdateFeedbackHandler : IRequestHandler<UpdateFeedbackCommand, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    public UpdateFeedbackHandler(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }
    public async Task<APIResponse> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
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
        var feedbackEntity = await _feedbackRepository.GetUserEventFeedback(request.Feedback.EventId, request.UserId);
        feedbackEntity.Rating = request.Feedback.Rating;
        feedbackEntity.Content = request.Feedback.Content;
        feedbackEntity.CreatedAt = DateTimeHelper.GetDateTimeNow();
        await _feedbackRepository.Update(feedbackEntity);
        if(await _unitOfWork.SaveChangesAsync() > 0)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.UpdateSuccesfully,
                Data = request.Feedback
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.BadRequest,
            Message = MessageCommon.UpdateFailed,
            Data = null
        };
    }
}
