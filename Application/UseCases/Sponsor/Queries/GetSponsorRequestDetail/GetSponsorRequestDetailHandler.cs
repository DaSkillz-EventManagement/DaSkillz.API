using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;
using Domain.DTOs.Sponsors;
using AutoMapper;
using Domain.DTOs.Events.ResponseDto;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequestDetail;

public class GetSponsorRequestDetailHandler : IRequestHandler<GetSponsorRequestDetailQueries, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ISponsorEventRepository _sponsorEventRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    public GetSponsorRequestDetailHandler(ISponsorEventRepository repository, IUserRepository userRepository, IMapper mapper, 
        IEventRepository eventRepository)
    {
        _sponsorEventRepository = repository;
        _userRepository = userRepository;
        _mapper = mapper;
        _eventRepository = eventRepository;
    }

    public async Task<APIResponse> Handle(GetSponsorRequestDetailQueries request, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetUserById(request.UserId);
        if (user == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageUser.UserNotFound,
                Data = null,
            };
        }
        var result = await _sponsorEventRepository.CheckSponsorEvent(request.EventId, request.UserId);
        if (result == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            };
        }
        SponsorEventDetailDto response = _mapper.Map<SponsorEventDetailDto>(result);
        var eventResponse = await _eventRepository.GetById(response.EventId);
        response.eventResponseDto = _mapper.Map<EventResponseDto>(eventResponse);
        return new APIResponse
        {
            Message = MessageCommon.Complete,
            StatusResponse = HttpStatusCode.OK,
            Data = response
    };
    }
}
