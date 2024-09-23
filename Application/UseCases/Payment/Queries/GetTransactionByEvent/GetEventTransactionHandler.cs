using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Payment.Response;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Queries.GetTransactionByEvent
{
    public class GetEventTransactionHandler : IRequestHandler<GetEventTransactionQuery, APIResponse>
    {
        private readonly IMapper mapper;
        private readonly ITransactionRepository _transactionRepository;

        public GetEventTransactionHandler(IMapper mapper, ITransactionRepository transactionRepository)
        {
            this.mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public async Task<APIResponse> Handle(GetEventTransactionQuery request, CancellationToken cancellationToken)
        {
            var exist = await _transactionRepository.getEventTransactionAsync(request.eventId);
            var map = mapper.Map<IEnumerable<TransactionResponseDto>>(exist);
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
