using Domain.DTOs.ParticipantDto;
using Domain.Entities;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using Domain.Repositories.Generic;
namespace Domain.Repositories;

public interface IParticipantRepository : IRepository<Participant>
{
    Task<List<HourlyPartitipant>> GetParticipationByHourAsync(Guid? eventId, DateTime startDate, DateTime endDate);
    Task<List<DailyParticipation>> GetParticipationByDayAsync(Guid? eventId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Participant?>> GetParticipantsByEventId(Guid eventId);
    Task<bool> IsExistedOnEvent(Guid userId, Guid eventId);
    Task<Participant?> GetParticipant(Guid userId, Guid eventId);
    Task<PagedList<Participant>> FilterDataParticipant(FilterParticipantDto filter);
    Task UpSert(Participant participant);
    Task<Participant?> GetDetailParticipant(Guid userId, Guid eventId);
    Task<bool> IsRole(Guid userId, Guid eventId, EventRole role);
    Task<bool> IsReachedCapacity(Guid eventId);

}
