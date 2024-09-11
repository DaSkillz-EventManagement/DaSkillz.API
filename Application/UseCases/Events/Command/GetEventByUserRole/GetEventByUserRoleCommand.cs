using Domain.DTOs.Events;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Events.Command.GetEventByUserRole
{
    public class GetEventByUserRoleCommand : IRequest<PagedList<EventResponseDto>>
    {
        public EventRole EventRole { get; set; }
        public Guid UserId { get; set; }
        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        public GetEventByUserRoleCommand(EventRole eventRole, int pageNo, int elementEachPage)
        {
            EventRole = eventRole;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
