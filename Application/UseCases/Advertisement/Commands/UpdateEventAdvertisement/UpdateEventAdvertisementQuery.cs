using Domain.DTOs;
using Domain.DTOs.Events.ResponseDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Advertisement.Commands.UpdateEventAdvertisement
{
    public class UpdateEventAdvertisementQuery : IRequest<EventResponseDto>
    {
        public Guid Id { get; set; }
        public AdvertisementRequestDto AdvertisementRequest { get; set; }

        public UpdateEventAdvertisementQuery(Guid id, AdvertisementRequestDto advertisementRequest)
        {
            Id = id;
            AdvertisementRequest = advertisementRequest;
        }
    }
}
