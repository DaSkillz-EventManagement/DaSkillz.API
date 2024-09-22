using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Pagination;
using MediatR;

namespace Application.UseCases.Events.Queries.GetEventParticipatedByUser
{
    public class GetEventParticipatedByUserQuery : IRequest<PagedList<EventResponseDto>>
    {
        public EventFilterObjectDto Filter { get; set; }
        public Guid UserId { get; set; }
        public int PageNo { get; set; }
        public int ElementEachPage { get; set; }

        public GetEventParticipatedByUserQuery(EventFilterObjectDto filter, Guid userId, int pageNo, int elementEachPage)
        {
            Filter = filter;
            UserId = userId;
            PageNo = pageNo;
            ElementEachPage = elementEachPage;
        }

        // Add UserId to the command, but it is not input by the user



    }
}
