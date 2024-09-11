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

namespace Application.UseCases.Events.Queries.GetBlobUri
{
    public class GetBlobUriQueryHandler : IRequestHandler<GetBlobUriQuery, SponsorLogoDto>
    {
        private readonly ILogoRepository _logoRepository;
        private readonly IMapper _mapper;

        public GetBlobUriQueryHandler(ILogoRepository logoRepository, IMapper mapper)
        {
            _logoRepository = logoRepository;
            _mapper = mapper;
        }

        public async Task<SponsorLogoDto> Handle(GetBlobUriQuery request, CancellationToken cancellationToken)
        {
            var result = await _logoRepository.GetByName(request.blobName);
            SponsorLogoDto response = _mapper.Map<SponsorLogoDto>(result);
            return response;
        }
    }
}
