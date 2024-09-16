using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEventParticipatedByUser
{
    public class GetEventParticipatedByUserQuery : IRequest<PagedList<EventResponseDto>>
    {
        public EventFilterObjectDto Filter { get; set; }

        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        // Add UserId to the command, but it is not input by the user
        public Guid UserId { get; set; }

        public GetEventParticipatedByUserQuery() 
        {
            Filter = new EventFilterObjectDto(); // Default initialization
            PageNo = 1; // Default page number
            ElementEachPage = 10; // Default elements per page
        }

        public GetEventParticipatedByUserQuery(EventFilterObjectDto filter, int pageNo, int elementEachPage)
        {
            Filter = filter;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }
    }
}
