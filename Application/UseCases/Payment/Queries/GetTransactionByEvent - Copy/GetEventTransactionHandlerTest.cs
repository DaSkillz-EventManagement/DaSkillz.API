using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Payment.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Queries.GetTransactionByEvent
{
    public class GetEventTransactionHandlerTest : IRequestHandler<GetEventTransactionQueryTest, APIResponse>
    {
        private readonly IMapper mapper;
        private readonly ITransactionRepository _transactionRepository;

        public GetEventTransactionHandlerTest(IMapper mapper, ITransactionRepository transactionRepository)
        {
            this.mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public async Task<APIResponse> Handle(GetEventTransactionQueryTest request, CancellationToken cancellationToken)
        {
            //var exist = await _transactionRepository.getEventTransactionAsync(request.eventId);

            var exist = await _transactionRepository.FilterTransactionsAsync(
                Guid.Parse("F5EC884A-72F6-476D-B357-982E89323313"), null, 3, null);

            int totalCount = exist?.Count() ?? 0;



            var map = mapper.Map<IEnumerable<MinimalTransactionResponseDto>>(exist);
            bool checkExist = exist != null;
            return new APIResponse
            {
                StatusResponse = checkExist ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                Message = checkExist ? MessageCommon.GetSuccesfully : MessageCommon.GetFailed,
                Data = checkExist ? new
                {
                    Total = totalCount,
                    Records = map
                } : null
            };
        }
    }
}
