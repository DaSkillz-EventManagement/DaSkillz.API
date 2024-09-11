using Domain.DTOs.Events;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Events.Command.GetFilteredEvent
{
    public class GetFilteredEventCommand : IRequest<PagedList<EventResponseDto>>
    {
        public EventFilterObjectDto Filter { get; set; }
        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        public GetFilteredEventCommand(EventFilterObjectDto filter, int pageNo, int elementEachPage)
        {
            Filter = filter;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
