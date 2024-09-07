using Domain.DTOs.Events;
using Domain.Models.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.GetEventParticipatedByUser
{
    public class GetEventParticipatedByUserCommand : IRequest<PagedList<EventResponseDto>>
    {
        public EventFilterObjectDto Filter { get; set; }
        public Guid UserId { get; set; }
        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        public GetEventParticipatedByUserCommand(EventFilterObjectDto filter, Guid userId, int pageNo, int elementEachPage)
        {
            Filter = filter;
            UserId = userId;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
