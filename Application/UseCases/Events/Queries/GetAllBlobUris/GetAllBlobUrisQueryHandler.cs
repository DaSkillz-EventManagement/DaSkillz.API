using Application.UseCases.Events.Command.CreateEvent;
using AutoMapper;
using Domain.DTOs.Sponsors;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetAllBlobUris
{
    public class GetAllBlobUrisQueryHandler : IRequestHandler<GetAllBlobUrisQuery, List<SponsorLogoDto>>
    {
        private readonly ILogoRepository _logoRepository;
        private readonly IMapper _mapper;

        public GetAllBlobUrisQueryHandler(ILogoRepository logoRepository, IMapper mapper)
        {
            _logoRepository = logoRepository;
            _mapper = mapper;
        }

        public async Task<List<SponsorLogoDto>> Handle(GetAllBlobUrisQuery request, CancellationToken cancellationToken)
        {
            List<SponsorLogoDto> response = new List<SponsorLogoDto>();
            var logos = await _logoRepository.GetAll();
            response = _mapper.Map<List<SponsorLogoDto>>(logos);
            return response;
        }
    }
}
