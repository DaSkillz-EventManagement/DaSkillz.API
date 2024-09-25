using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Sponsors;
using Domain.Enum.Sponsor;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Sponsor.Commands.DeleteSponsorRequest;

public class DeleteSponsorRequestHandler : IRequestHandler<DeleteSponsorRequestCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISponsorEventRepository _sponsorEventRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    public DeleteSponsorRequestHandler(IUnitOfWork unitOfWork, ISponsorEventRepository repository, IMapper mapper, IUserRepository userRepository)
    {
        _sponsorEventRepository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    public async Task<APIResponse> Handle(DeleteSponsorRequestCommand request, CancellationToken cancellationToken)
    {
        APIResponse response = new APIResponse();
        var requestEvent = await _sponsorEventRepository.CheckSponsorEvent(request.EventId, request.UserId);
        if (!requestEvent.Status.Equals(SponsorRequest.Confirmed.ToString()) || !requestEvent.Status.Equals(SponsorRequest.Reject.ToString()))
        {
            var result = await _sponsorEventRepository.DeleteSponsorRequest(request.EventId, request.UserId);
            if (result != null)
            {
                var user = await _userRepository.GetById(result!.UserId!);
                SponsorEventDto dto = _mapper.Map<SponsorEventDto>(result);
                dto.FullName = user!.FullName!;
                dto.Email = user!.Email!;
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.DeleteSuccessfully;
                response.Data = dto;
            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.DeleteFailed;
            }
            return response;
        }
        return new APIResponse
        {
            Message = MessageCommon.DeleteFailed,
            StatusResponse = HttpStatusCode.BadRequest,
        };
    }
}
