﻿using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Feedbacks;
using Domain.Models.Pagination;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Feedbacks.Queries.GetEventFeedbacks;

public class GetEventFeedbacksHandler : IRequestHandler<GetEventFeedbacksQueries, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;
    private readonly IEventRepository _eventRepository;
    public GetEventFeedbacksHandler(IFeedbackRepository feedbackRepository, IMapper mapper, IEventRepository eventRepository)
    {
        _feedbackRepository = feedbackRepository;
        _mapper = mapper;
        _eventRepository = eventRepository;
    }
    public async Task<APIResponse> Handle(GetEventFeedbacksQueries request, CancellationToken cancellationToken)
    {
        var user = await _eventRepository.IsOwner(request.UserId, request.EventId);
        if (user == null)
        if(!user)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageParticipant.NotOwner,
                Data = null
            };
        }
        var feedbacks = await _feedbackRepository.GetFeedbackByEventIdAndStar(request.EventId, request.NumberOfStar, request.Page, request.EachPage);
        var result = _mapper.Map<PagedList<FeedbackEvent>>(feedbacks);
        if (result != null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            };
        }
        return new APIResponse
        {
            StatusResponse = HttpStatusCode.NotFound,
            Message = MessageCommon.NotFound,
            Data = null
        };
    }
}
