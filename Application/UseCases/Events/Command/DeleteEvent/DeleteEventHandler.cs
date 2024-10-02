using Application.Abstractions.Caching;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Repositories;
using Event_Management.Domain.Enum;
using MediatR;

namespace Application.UseCases.Events.Command.DeleteEvent
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventCommand, bool>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRedisCaching _redisCaching;

        public DeleteEventHandler(IEventRepository eventRepository, IUserRepository userRepository, IRedisCaching redisCaching)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _redisCaching = redisCaching;
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
                DateTimeOffset startDate = DateTimeOffset.FromUnixTimeMilliseconds(existEvent.StartDate);

                //Xóa tất cả cache mà có từ khóa nằm trong cacheKey
                var invalidateCache = await _redisCaching.SearchKeysAsync("getEv");
                if (invalidateCache != null)
                {
                    foreach (var key in invalidateCache)
                        await _redisCaching.DeleteKeyAsync(key);
                }

                if (isOwner)
                {
                    if (startDate.CompareTo(DateTime.Now.AddHours(6)) < 0)
                    {
                        return await _eventRepository.ChangeEventStatus(request.EventId, EventStatus.Cancel);
                    }

                    return await _eventRepository.ChangeEventStatus(request.EventId, EventStatus.Deleted);
                }
                if (!isOwner && userInfo!.RoleId == ((int)UserRole.Admin))
                {
                    return await _eventRepository.ChangeEventStatus(request.EventId, EventStatus.Aborted);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
