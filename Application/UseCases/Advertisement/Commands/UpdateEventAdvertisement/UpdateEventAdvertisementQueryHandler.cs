using Application.UseCases.Advertisement.Queries.GetEventAdvertisement;
using AutoMapper;
using Domain.DTOs.Events.ResponseDto;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Advertisement.Commands.UpdateEventAdvertisement
{
    public class UpdateEventAdvertisementQueryHandler : IRequestHandler<UpdateEventAdvertisementQuery, EventResponseDto>
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMapper _mapper;

        public UpdateEventAdvertisementQueryHandler(IAdvertisementRepository advertisementRepository, IMapper mapper)
        {
            _advertisementRepository = advertisementRepository;
            _mapper = mapper;
        }

        public async Task<EventResponseDto> Handle(UpdateEventAdvertisementQuery request, CancellationToken cancellationToken)
        {
            var advertisement = await _advertisementRepository.GetById(request.Id);
            _mapper.Map(request.AdvertisementRequest, advertisement);
            await _advertisementRepository.Update(advertisement);

            
            var eventResponse = _mapper.Map<EventResponseDto>(advertisement);
            return eventResponse;
        }
    }
}
