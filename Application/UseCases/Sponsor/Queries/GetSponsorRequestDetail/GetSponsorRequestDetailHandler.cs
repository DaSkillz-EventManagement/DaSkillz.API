using Domain.Repositories;
using Domain.Models.Response;
using MediatR;
using Application.ResponseMessage;
using System.Net;

namespace Application.UseCases.Sponsor.Queries.GetSponsorRequestDetail;

public class GetSponsorRequestDetailHandler : IRequestHandler<GetSponsorRequestDetailQueries, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ISponsorEventRepository _sponsorEventRepository;
    public GetSponsorRequestDetailHandler(ISponsorEventRepository repository, IUserRepository userRepository)
    {
        _sponsorEventRepository = repository;      
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(GetSponsorRequestDetailQueries request, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetUserById(request.UserId);
        if(user == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.BadRequest,
                Message = MessageUser.UserNotFound,
                Data = null,
        };
        }
        var result =  await _sponsorEventRepository.CheckSponsorEvent(request.EventId, request.UserId);
        if(result == null)
        {
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.NotFound,
                Message = MessageCommon.NotFound,
                Data = null
            };
        }
        return new APIResponse
        {
            Message = MessageCommon.Complete,
            StatusResponse = HttpStatusCode.OK,
            Data = result
        };
    }
}
