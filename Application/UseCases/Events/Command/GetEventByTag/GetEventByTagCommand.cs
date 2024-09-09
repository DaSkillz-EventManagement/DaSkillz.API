using Domain.DTOs.Events;
using Domain.Models.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Command.GetEventByTag
{
    public class GetEventByTagCommand: IRequest<PagedList<EventResponseDto>>
    {
        public List<int> TagIds { get; set; }
        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        public GetEventByTagCommand(List<int> tagIds, int pageNo, int elementEachPage)
        {
            TagIds = tagIds;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
