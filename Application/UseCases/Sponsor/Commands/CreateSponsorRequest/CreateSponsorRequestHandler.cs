using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.Entities;
using Domain.Enum.Sponsor;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Event_Management.Domain.Enum.Sponsor;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Application.UseCases.Sponsor.Commands.CreateSponsorRequest;

public class CreateSponsorRequestHandler : IRequestHandler<CreateSponsorRequestCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISponsorEventRepository _sponsorEventRepository;
    private readonly IEventRepository _eventRepository;
    public CreateSponsorRequestHandler(IMapper mapper, IUnitOfWork unitOfWork,
        ISponsorEventRepository repository, IEventRepository eventRepository)
    {
        _sponsorEventRepository = repository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<APIResponse> Handle(CreateSponsorRequestCommand sponsorEvent,
        CancellationToken cancellationToken)
    {
        var newSponsorRequest = new SponsorEvent();
        newSponsorRequest.EventId = sponsorEvent.Sponsor.EventId;

        var eventEntity = await  _eventRepository.GetById(sponsorEvent.Sponsor.EventId);
        
            newSponsorRequest.UserId = sponsorEvent.UserId;
            newSponsorRequest.SponsorType = sponsorEvent.Sponsor.SponsorType;
            newSponsorRequest.Message = sponsorEvent.Sponsor.Message;
            newSponsorRequest.Amount = sponsorEvent.Sponsor.Amount;

            newSponsorRequest.CreatedAt = DateTimeHelper.GetDateTimeNow();
            newSponsorRequest.UpdatedAt = DateTimeHelper.GetDateTimeNow();
            newSponsorRequest.Status = SponsorRequest.Processing.ToString();
            newSponsorRequest.IsSponsored = false;
            await _sponsorEventRepository.Add(newSponsorRequest);
        try
        {
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                Message = MessageCommon.Complete,
                StatusResponse = HttpStatusCode.OK,
                Data = newSponsorRequest
            };
        } catch (Exception ex)
        {
            return new APIResponse
            {
                Message = MessageCommon.SavingFailed,
                StatusResponse = HttpStatusCode.BadRequest,
                Data = ex.Message
            };
        }
    }
}
