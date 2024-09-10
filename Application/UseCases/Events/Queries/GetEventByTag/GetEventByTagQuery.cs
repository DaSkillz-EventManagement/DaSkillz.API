using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetEventByTag
{
    public class GetEventByTagQuery : IRequest<PagedList<EventResponseDto>>
    {
        public List<int> TagIds { get; set; }
        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        public GetEventByTagQuery()
        {
            TagIds = new List<int>();
            PageNo = 1;
            ElementEachPage = 10;
        }

        public GetEventByTagQuery(List<int> tagIds, int pageNo, int elementEachPage)
        {
            TagIds = tagIds;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
