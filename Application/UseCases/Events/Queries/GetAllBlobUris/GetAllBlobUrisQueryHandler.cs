using Application.Abstractions.Caching;
using AutoMapper;
using Domain.DTOs.Sponsors;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Events.Queries.GetAllBlobUris
{
    public class GetAllBlobUrisQueryHandler : IRequestHandler<GetAllBlobUrisQuery, List<SponsorLogoDto>>
    {
        private readonly ILogoRepository _logoRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _redisCaching;

        public GetAllBlobUrisQueryHandler(ILogoRepository logoRepository, IMapper mapper, IRedisCaching redisCaching)
        {
            _logoRepository = logoRepository;
            _mapper = mapper;
            _redisCaching = redisCaching;
        }

        public async Task<List<SponsorLogoDto>> Handle(GetAllBlobUrisQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = $"GetAllBlob";
            //var cachedDataString = await _redisCaching.GetAsync<List<SponsorLogoDto>>(cacheKey);
            //if (cachedDataString != null)
            //{
            //    return cachedDataString;
            //}

            List<SponsorLogoDto> response = new List<SponsorLogoDto>();
            var logos = await _logoRepository.GetAll();
            response = _mapper.Map<List<SponsorLogoDto>>(logos);

            //await _redisCaching.SetAsync(cacheKey, response, 10);

            return response;
        }
    }
}
