using Domain.DTOs.Events.ResponseDto;
using MediatR;

namespace Application.UseCases.Events.Queries.GetTopCreatorsByEventCount
{
    public class GetTopCreatorsByEventQuery : IRequest<List<EventCreatorLeaderBoardDto>>
    {
    }
}
