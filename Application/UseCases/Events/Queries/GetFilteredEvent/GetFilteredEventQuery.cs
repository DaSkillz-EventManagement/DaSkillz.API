using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Events.Queries.GetFilteredEvent
{
    public class GetFilteredEventQuery : IRequest<PagedList<EventResponseDto>>
    {
        public EventFilterObjectDto Filter { get; set; }
        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        // Parameterless constructor for model binding
        public GetFilteredEventQuery()
        {
            Filter = new EventFilterObjectDto(); // Default initialization
            PageNo = 1; // Default page number
            ElementEachPage = 10; // Default elements per page
        }
        public GetFilteredEventQuery(EventFilterObjectDto filter, int pageNo, int elementEachPage)
        {
            Filter = filter;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
