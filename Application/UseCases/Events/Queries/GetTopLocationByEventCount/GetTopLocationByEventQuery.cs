using Domain.DTOs.Events.ResponseDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetTopLocationByEventCount
{
    public class GetTopLocationByEventQuery : IRequest<List<EventLocationLeaderBoardDto>>
    {
    }
}
