using Domain.DTOs.Events;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.GetEventByUserRole
{
    public class GetEventByUserRoleCommand : IRequest<PagedList<EventResponseDto>>
    {
        public EventRole EventRole { get; set; }
        public Guid UserId { get; set; }
        public int PageNo {  get; set; }
        public int ElementEachPage { get; set; }

        public GetEventByUserRoleCommand(EventRole eventRole, int pageNo, int elementEachPage)
        {
            EventRole = eventRole;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
