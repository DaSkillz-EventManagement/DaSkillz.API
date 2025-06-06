﻿using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Sponsors;
using Domain.Entities;
using Domain.Enum.Sponsor;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Sponsor.Commands.CreateSponsorRequest;

public class CreateSponsorRequestHandler : IRequestHandler<CreateSponsorRequestCommand, APIResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISponsorEventRepository _sponsorEventRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    public CreateSponsorRequestHandler(IMapper mapper, IUnitOfWork unitOfWork,
        ISponsorEventRepository repository, IEventRepository eventRepository, IUserRepository userRepository)
    {
        _sponsorEventRepository = repository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    public async Task<APIResponse> Handle(CreateSponsorRequestCommand sponsorEvent,
        CancellationToken cancellationToken)
    {
        var newSponsorRequest = new SponsorEvent();
        newSponsorRequest.EventId = sponsorEvent.Sponsor.EventId;
        var isSponsor = await _sponsorEventRepository.CheckSponsorEvent(sponsorEvent.Sponsor.EventId, sponsorEvent.UserId);
        if(isSponsor != null)
        {
            return new APIResponse
            {
                Message = MessageCommon.SavingFailed,
                StatusResponse = HttpStatusCode.BadRequest,
                Data = null
            };
        }
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
            var user = await _userRepository.GetById(sponsorEvent.UserId);
            SponsorEventDto response = _mapper.Map<SponsorEventDto>(newSponsorRequest);
            response.FullName = user!.FullName!;
            response.Email = user!.Email!;
            return new APIResponse
            {
                Message = MessageCommon.Complete,
                StatusResponse = HttpStatusCode.OK,
                Data = response
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
