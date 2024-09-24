using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using Domain.DTOs.User.Response;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using Domain.Repositories.Generic;

namespace Domain.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {

        Task<bool> IsOwner(Guid userId, Guid eventId);


        //Get All event with search, paging and sort.
        public Task<PagedList<Event>> GetFilteredEvent(EventFilterObjectDto filter, int pageNo, int elementEachPage);

        //Get List of events that user have participated
        public Task<PagedList<Event>> GetUserParticipatedEvents(EventFilterObjectDto filter, Guid userId, int pageNo, int elementEachPage);

        //User past and incoming events
        public Task<List<Event>> UserPastEvents(Guid userId);
        public Task<List<Event>> UserIncomingEvents(Guid userId);

        //AUTO update status for event
        //Update status: On going
        public bool UpdateEventStatusToOnGoing(Guid eventId);
        public bool UpdateEventStatusToOnGoing();
        //Update status: Ended
        public bool UpdateEventStatusToEnded(Guid eventId);
        public bool UpdateEventStatusToEnded();

        Task<bool> ChangeEventStatus(Guid eventId, EventStatus status);

        public Task<List<EventCreatorLeaderBoardDto>> GetTop10CreatorsByEventCount();
        public Task<List<EventLocationLeaderBoardDto>> GetTop10LocationByEventCount();


        // get Event
        //public Task<Event> getAllEventInfo(Guid eventId);
        public Task<PagedList<Event>> getEventByUserRole(EventRole eventRole, Guid userId, int pageNo, int elementEachPage);
        public Task<PagedList<Event>> GetEventsByTag(int tagId, int pageNo, int elementEachPage);
        public Task<PagedList<Event>> GetEventsByListTags(List<int> tagIds, int pageNo, int elementEachPage);
        
        Task<List<Event>> GetUserHostEvent(Guid userId);
        Task<List<Event>> GetEventsByIdsAsync(List<Guid> eventIds);
        Task<CreatedByUserDto> GetHostInfo(Guid userId);

        EventPreviewDto ToEventPreview(Event entity);
        CreatedByUserDto getHostInfo(Guid userId);
        EventResponseDto ToResponseDto(Event eventEntity);
        Task<Dictionary<string, int>> CountByStatus();
        Task<List<EventPerMonth>> EventsPerMonth(DateTime startDate, DateTime endDate);
    }
}
