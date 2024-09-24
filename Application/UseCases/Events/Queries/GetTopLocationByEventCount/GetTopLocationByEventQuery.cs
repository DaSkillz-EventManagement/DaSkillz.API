using Domain.DTOs.Events.ResponseDto;
using MediatR;

namespace Application.UseCases.Events.Queries.GetTopLocationByEventCount
{
    public class GetTopLocationByEventQuery : IRequest<List<EventLocationLeaderBoardDto>>
    {
    }
}
