using Application.Abstractions.Caching;
using AutoMapper;
using Domain.DTOs.Sponsors;
using Domain.Repositories;
using MediatR;

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
            //string cacheKey = $"GetBlobUri_{request.BlobName}";
            //var cachedDataString = await _redisCaching.GetAsync<SponsorLogoDto>(cacheKey);
            //if (cachedDataString != null)
            //{
            //    return cachedDataString;
            //}

            var result = await _logoRepository.GetByName(request.BlobName);
            SponsorLogoDto response = _mapper.Map<SponsorLogoDto>(result);

            //await _redisCaching.SetAsync(cacheKey, response, 10);


            return response;
        }
    }
}
