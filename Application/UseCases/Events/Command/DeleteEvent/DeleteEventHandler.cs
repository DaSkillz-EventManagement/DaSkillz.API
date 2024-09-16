using Application.Helper;
using Application.UseCases.Events.Command.CreateEvent;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Event_Management.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.DeleteEvent
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventCommand, bool>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;

        public DeleteEventHandler(IEventRepository eventRepository, IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            
            try
            {
                bool isOwner = await _eventRepository.IsOwner(request.EventId, request.UserId);
                //not Complete
                //bool isDeletable = await IsDeletable(eventId);
                var userInfo = await _userRepository.GetById(request.UserId);
                Event? existEvent = await _eventRepository.GetById(request.EventId);
                if (existEvent == null || !existEvent.Status!.Equals(EventStatus.NotYet.ToString())) // || !isDeletable
                {
                    return false;
                }
                if (isOwner)
                {
                    if (existEvent.StartDate.CompareTo(DateTime.Now.AddHours(6)) < 0)
                    {
                        return await _eventRepository.ChangeEventStatus(request.EventId, EventStatus.Cancel);
                    }
                    return await _eventRepository.ChangeEventStatus(request.EventId, EventStatus.Deleted);
                }
                if (!isOwner && userInfo!.RoleId == ((int)UserRole.Admin))
                {
                    return await _eventRepository.ChangeEventStatus(request.EventId, EventStatus.Aborted);
                }

                //await EventHelper.InvalidateEventCacheAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
