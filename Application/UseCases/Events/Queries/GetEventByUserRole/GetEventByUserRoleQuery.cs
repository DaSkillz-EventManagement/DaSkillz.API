using Domain.DTOs.Events.ResponseDto;
using Domain.Enum.Events;
using Domain.Models.Pagination;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
