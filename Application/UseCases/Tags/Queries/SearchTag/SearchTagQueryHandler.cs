using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Tag;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;

namespace Application.UseCases.Tags.Queries.SearchTag
{

    public class SearchTagQueryHandler : IRequestHandler<SearchTagQuery, APIResponse>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public SearchTagQueryHandler(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(SearchTagQuery request, CancellationToken cancellationToken)
        {
            var filteredTags = await _tagRepository.SearchTag(request.searchTag!);

            // Convert filtered tags to TagDto using AutoMapper
            var tagDtos = _mapper.Map<List<Tag>, List<TagDto>>(filteredTags);

            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.ReturnListHasValue,
                Data = tagDtos
            };
        }
    }
}
