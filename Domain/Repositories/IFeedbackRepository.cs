using Domain.Entities;
using Domain.Models.Pagination;
using Domain.Repositories.Generic;

namespace Domain.Repositories;

public interface IFeedbackRepository : IRepository<Feedback>
{
    Task<PagedList<Feedback>?> GetFeedbackByEventIdAndStar(Guid eventId, int? numOfStar, int page, int eachPage);
    Task<Feedback> GetUserEventFeedback(Guid eventId, Guid userId);
    Task<PagedList<Feedback>?> GetAllUserFeebacks(Guid userId, int page, int eachPage);
}
