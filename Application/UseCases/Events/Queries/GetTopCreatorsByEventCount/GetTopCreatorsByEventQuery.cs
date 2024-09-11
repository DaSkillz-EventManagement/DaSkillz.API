using Domain.DTOs.Events.ResponseDto;
using Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetTopCreatorsByEventCount
{
    public class GetTopCreatorsByEventQuery : IRequest<List<EventCreatorLeaderBoardDto>>
    {
    }
}
