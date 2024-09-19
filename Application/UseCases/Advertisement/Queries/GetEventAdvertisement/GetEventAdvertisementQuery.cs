using Domain.DTOs.Events.ResponseDto;
using Domain.DTOs.Sponsors;
using Domain.Models.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Advertisement.Queries.GetEventAdvertisement
{
    public class GetEventAdvertisementQuery : IRequest<List<EventResponseDto>>
    {
    }
}
