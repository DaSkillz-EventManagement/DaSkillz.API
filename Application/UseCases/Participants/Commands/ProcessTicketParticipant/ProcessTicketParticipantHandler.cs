using Application.Abstractions.Email;
using Application.Helper;
using Application.ResponseMessage;
using AutoMapper;
using Domain.Constants.Mail;
using Domain.DTOs.ParticipantDto;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Elastic.Clients.Elasticsearch.Snapshot;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Participants.Commands.ProcessTicketParticipant
{
    public class ProcessTicketParticipantHandler : IRequestHandler<ProcessTicketParticipantCommand, APIResponse>
    {
        private readonly IEventRepository _eventRepo;
        private readonly IParticipantRepository _participantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _sendMail;

        public ProcessTicketParticipantHandler(IEventRepository eventRepo, IParticipantRepository participantRepository, IUnitOfWork unitOfWork,
            IUserRepository userRepository, IEmailService sendMail)
        {
            _eventRepo = eventRepo;
            _participantRepository = participantRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _sendMail = sendMail;
        }
        public async Task<APIResponse> Handle(ProcessTicketParticipantCommand request, CancellationToken cancellationToken)
        {
            var isOwner = await _eventRepo.IsOwner(request.ParticipantTicket.EventId, request.UserId);
            if (!isOwner)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.NotOwner,
                    Data = null
                };
            }
            if (!Utilities.IsValidParticipantStatus(request.ParticipantTicket.Status))
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageParticipant.ParticipantStatusNotValid,
                    Data = null
                };
            }
            var participant = await _participantRepository.GetParticipant(request.ParticipantTicket.UserId, request.ParticipantTicket.EventId);

            if (participant == null)
            {
                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                };
            }
            participant.Status = request.ParticipantTicket.Status;

            await _participantRepository.Update(participant);
            var currentEvent = await _eventRepo.GetById(request.ParticipantTicket.EventId);
            var Owner = await _userRepository.GetById(currentEvent!.CreatedBy!);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                #region sendMail
                var user = await _userRepository.GetById(request.UserId);
                await _sendMail.SendEmailTicket(MailConstant.TicketMail.PathTemplate, MailConstant.TicketMail.Title, new TicketModel()
                {
                    EventId = request.ParticipantTicket.EventId,
                    UserId = (Guid)currentEvent.CreatedBy!,
                    Email = user!.Email,
                    RoleEventId = (int)participant.RoleEventId!,
                    FullName = user.FullName,
                    Avatar = Owner!.Avatar,
                    EventName = currentEvent?.EventName,
                    Location = currentEvent?.Location,
                    LocationUrl = currentEvent?.LocationUrl,
                    LocationAddress = currentEvent?.LocationAddress,
                    LogoEvent = currentEvent?.Image,
                    OrgainzerName = Owner.FullName,
                    StartDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.StartDate).DateTime,
                    EndDate = DateTimeOffset.FromUnixTimeMilliseconds(currentEvent!.EndDate).DateTime,
                    Time = DateTimeHelper.GetTimeRange(DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.StartDate).DateTime, DateTimeOffset.FromUnixTimeMilliseconds(currentEvent.EndDate).DateTime),
                    Message = TicketMailConstant.MessageMail.ElementAt((int)participant.RoleEventId! - 1),
                    TypeButton = Utilities.GetTypeButton((int)participant.RoleEventId!),
                });
                #endregion

                return new APIResponse()
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageParticipant.ProcessParticipant
                };
            }

            return new APIResponse()
            {
                StatusResponse = HttpStatusCode.NotModified,
                Message = MessageParticipant.ProcessParticipantFailed
            };
        }
    }
}
