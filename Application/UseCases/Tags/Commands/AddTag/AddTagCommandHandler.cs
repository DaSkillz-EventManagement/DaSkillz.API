using Application.ResponseMessage;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Tags.Commands.AddTag
{
    public class AddTagCommandHandler : IRequestHandler<AddTagCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public AddTagCommandHandler(IUnitOfWork unitOfWork, ITagRepository tagRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(AddTagCommand request, CancellationToken cancellationToken)
        {
            var existTag = await _tagRepository.GetTagByName(request.TagName);
            if (existTag == null)
            {
                var tagEntity = new Tag {
                    TagName = request.TagName,
                };

                await _tagRepository.Add(tagEntity);
                await _unitOfWork.SaveChangesAsync();
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.OK,
                    Message = MessageCommon.CreateSuccesfully,
                    Data = tagEntity
                };
            }

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.CreateFailed,
                Data = null
            };

        }
    }
}
