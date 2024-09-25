using Application.ResponseMessage;
using Domain.DTOs.Feedbacks;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Feedbacks.Queries.GetAllUserFeebacks;

public class GetAllUserFeebacksHandler : IRequestHandler<GetAllUserFeebacksQueries, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUserRepository _userRepository;
    public GetAllUserFeebacksHandler(IFeedbackRepository feedbackRepository, IUserRepository userRepository)
    {
        _feedbackRepository = feedbackRepository;
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(GetAllUserFeebacksQueries request, CancellationToken cancellationToken)
    {
        var result = await _feedbackRepository.GetAllUserFeebacks(request.UserId, request.Page, request.EachPage);
        List<FeedbackView> response = new List<FeedbackView>();
        foreach (var item in result!)
        {
            response.Add(await ToFeebackView(item));
        }
        PagedList<FeedbackView> temp = new PagedList<FeedbackView>
        {
            Items = response,
            TotalItems = response.Count,
            CurrentPage = request.Page,
            EachPage = request.EachPage
        };
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.OK,
            Message = MessageCommon.Complete,
            Data = temp
        };
    }
    private async Task<FeedbackView> ToFeebackView(Feedback feedback)
    {
        return new FeedbackView
        {
            EventId = feedback.EventId,
            Content = feedback.Content,
            Rating = feedback.Rating,
            CreatedBy = await getUserInfo(feedback.UserId)
        };
    }
    private async Task<CreatedByUserDto> getUserInfo(Guid userId)
    {
        var userInfo = await _userRepository.GetById(userId);
        CreatedByUserDto response = new CreatedByUserDto();
        response.avatar = userInfo!.Avatar;
        response.Name = userInfo.FullName;
        response.Id = userInfo.UserId;
        return response;
    }
}
