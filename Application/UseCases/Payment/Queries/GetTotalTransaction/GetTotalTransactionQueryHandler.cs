using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Payment.Response;
using Domain.Enum.Payment;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetTotalTransaction
{
    public class GetTotalTransactionQueryHandler : IRequestHandler<GetTotalTransactionQuery, APIResponse>

    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetTotalTransactionQueryHandler(ITransactionRepository transactionRepository, IUserRepository userRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetTotalTransactionQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.FilterTransactionsAsync(
                 request.eventId,
                 request.userId, 1, null);

            bool isPremiumUser = await _userRepository.CheckUserIsPremium(request.userId);

            // Initialize totals
            decimal totalTicketPaidAmount = 0;
            decimal totalSponsorAmount = 0;

            foreach (var transaction in transactions)
            {
                // Parse the Amount from string to decimal
                if (decimal.TryParse(transaction.Amount, out decimal amount))
                {
                    // Calculate totals based on SubscriptionType
                    if (transaction.SubscriptionType == (int)PaymentType.TICKET) 
                    {
                        totalTicketPaidAmount += amount;
                    }
                    else if (transaction.SubscriptionType == (int)PaymentType.SPONSOR) 
                    {
                        totalSponsorAmount += amount;
                    }
                }
            }

            // Calculate total commission charge (5% of the total amount)
            decimal totalCommissionCharge = !isPremiumUser
                ? (totalTicketPaidAmount) * 0.05m
                : 0;

            // Prepare the response DTO
            var result = new TotalTransactionResponseDto
            {
                EventId = request.eventId,
                UserId = request.userId,
                TotalTicketPaidAmount = totalTicketPaidAmount,
                TotalSponsorAmount = totalSponsorAmount,
                TotalCommissionCharge = isPremiumUser ? totalCommissionCharge : null
            };

            return new APIResponse
            {
                StatusResponse = System.Net.HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = result
            };
        }
    }
}
