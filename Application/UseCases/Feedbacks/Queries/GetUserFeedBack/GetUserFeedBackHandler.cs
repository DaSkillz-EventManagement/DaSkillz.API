using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Feedbacks;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Feedbacks.Queries.GetUserFeedBack
{
    public class GetUserFeedBackHandler : IRequestHandler<GetUserFeedBackQueries, APIResponse>
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        public GetUserFeedBackHandler(IFeedbackRepository feedbackRepository, IUserRepository userRepository)
        {
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
        }
        public async Task<APIResponse> Handle(GetUserFeedBackQueries request, CancellationToken cancellationToken)
        {
            var result = await _feedbackRepository.GetUserEventFeedback(request.EventId, request.UserId);
            if(result != null)
            {
                FeedbackView temp = new FeedbackView
                {
                    EventId = request.EventId,
                    Content = result.Content,
                    Rating = result.Rating,
                    CreatedBy = await getUserInfo(result.UserId)
                };
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.Complete,
                    Data = temp
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
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
}
