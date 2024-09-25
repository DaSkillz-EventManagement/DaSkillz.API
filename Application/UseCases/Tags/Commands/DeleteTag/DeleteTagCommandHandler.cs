using Application.ResponseMessage;
using AutoMapper;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;

namespace Application.UseCases.Tags.Commands.DeleteTag
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public DeleteTagCommandHandler(IUnitOfWork unitOfWork, ITagRepository tagRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            await _tagRepository.Delete(request.tagId);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.DeleteSuccessfully,
                Data = null
            };
        }
    }
}
