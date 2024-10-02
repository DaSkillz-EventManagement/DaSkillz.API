using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using Domain.Entities;

namespace Infrastructure.ExternalServices.Quartz.CertificateScheduler
{

    public class ProvideCertificateAfterEventEnded : IJob
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly ICertificateRepository certificateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProvideCertificateAfterEventEnded(IUserRepository userRepository, IEventRepository eventRepository, IParticipantRepository participantRepository, ICertificateRepository certificateRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _participantRepository = participantRepository;
            this.certificateRepository = certificateRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var endedEvents = await _eventRepository.GetEndedEventsAsync();

            foreach (var endedEvent in endedEvents)
            {
                var participants = await _participantRepository.GetParticipantsByEventId(endedEvent.EventId);

                if (participants == null || !participants.Any()) return;

                foreach (var participant in participants)
                {
                    var exist = await certificateRepository.CheckIfUserHaveCertificate(participant!.UserId, endedEvent.EventId);
                    if (exist) continue;
                    var user = await _userRepository.GetById(participant.UserId);
                    if (user != null)
                    {
                        var certificate = new Certificate
                        {
                            UserId = user.UserId,
                            EventId = endedEvent.EventId,
                            IssueDate = DateTime.UtcNow
                        };

                        await certificateRepository.Add(certificate);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }
            }

           
        }
    }
}
