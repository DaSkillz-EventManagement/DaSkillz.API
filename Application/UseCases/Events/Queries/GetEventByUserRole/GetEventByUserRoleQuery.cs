using Domain.DTOs.Events.ResponseDto;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEventByUserRole
{
    public class GetEventByUserRoleQuery : IRequest<PagedList<EventResponseDto>>
    {
        public EventRole EventRole { get; set; }
        public Guid UserId { get; set; }
        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        public GetEventByUserRoleQuery(EventRole eventRole, int pageNo, int elementEachPage)
        {
            EventRole = eventRole;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
