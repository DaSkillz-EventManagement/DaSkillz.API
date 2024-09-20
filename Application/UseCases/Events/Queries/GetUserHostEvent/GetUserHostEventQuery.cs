using Domain.DTOs.Events;
using Domain.DTOs.Events.ResponseDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetUserHostEvent
{
    public class GetUserHostEventQuery : IRequest<List<EventPreviewDto>>
    {
        public Guid UserId { get; set; }

        public GetUserHostEventQuery()
        {
        }

        public GetUserHostEventQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
