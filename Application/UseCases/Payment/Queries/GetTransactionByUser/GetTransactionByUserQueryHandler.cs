using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Payment.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Queries.GetTransactionByUser
{
    public class GetTransactionByUserQueryHandler : IRequestHandler<GetTransactionByUserQuery, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetTransactionByUserQueryHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetTransactionByUserQuery request, CancellationToken cancellationToken)
        {
            var exist = await _transactionRepository.getTransactionByUserIdAsync(request.userId);
            var map = _mapper.Map<IEnumerable<TransactionResponseDto>>(exist);
            bool checkExist = exist != null;
            return new APIResponse
            {
                StatusResponse = checkExist ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                Message = checkExist ? MessageCommon.GetSuccesfully : MessageCommon.GetFailed,
                Data = checkExist ? map : null,
            };
        }
    }
}
