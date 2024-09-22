using Application.ResponseMessage;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using MediatR;
using System.Net;
using Elastic.Clients.Elasticsearch;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.UseCases.Tags.Queries.GetById
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public GetTagByIdQueryHandler(IUnitOfWork unitOfWork, ITagRepository tagRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
        public async Task<APIResponse> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _tagRepository.GetById(request.tagId);
            if (result == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound,
                };
            }
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = result
            };
        }
    }
}
