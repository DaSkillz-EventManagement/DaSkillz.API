using Application.Abstractions.Caching;
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
        private readonly IRedisCaching _redisCaching;

        public GetBlobUriQueryHandler(ILogoRepository logoRepository, IMapper mapper, IRedisCaching redisCaching)
        {
            _logoRepository = logoRepository;
            _mapper = mapper;
            _redisCaching = redisCaching;
        }

        public async Task<SponsorLogoDto> Handle(GetBlobUriQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetBlobUri_{request.blobName}";
            var cachedDataString = await _redisCaching.GetAsync<SponsorLogoDto>(cacheKey);
            if (cachedDataString != null)
            {
                return cachedDataString;
            }

            var result = await _logoRepository.GetByName(request.blobName);
            SponsorLogoDto response = _mapper.Map<SponsorLogoDto>(result);

            await _redisCaching.SetAsync(cacheKey, response, 10);


            return response;
        }
    }
}
