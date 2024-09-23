using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Tag;
using Domain.Models.Pagination;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Tags.Queries.GetAllTag
{
    public class GetAllTagQueryHandler : IRequestHandler<GetAllTagQuery, APIResponse>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public GetAllTagQueryHandler(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetAllTagQuery request, CancellationToken cancellationToken)
        {
            APIResponse response = new APIResponse();
            var result = await _tagRepository.GetAll(request.page, request.eachPage, "TagName"); ;


            if (result.Any())
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.Complete;
                response.Data = _mapper.Map<PagedList<TagDto>>(result);
            }
            else
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageCommon.NotFound;
            }
            return response;
        }
    }
}
