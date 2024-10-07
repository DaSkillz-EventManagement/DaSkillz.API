using Application.ResponseMessage;
using Application.UseCases.AdvertiseEvents.Queries.GetAdEventStatistic;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.User.Queries.GetAccountStatistic
{
    public class GetAccountStatisticQueryHandler : IRequestHandler<GetAccountStatisticQuery, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;

        public GetAccountStatisticQueryHandler(ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public async Task<APIResponse> Handle(GetAccountStatisticQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var isAdmin = await _userRepository.IsAdmin(request.UserId);
            if (!isAdmin)
            {
                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageUser.UserNotAdmin;
                response.Data = null;
                return response;
            } else
            {
                var listAccount = await _transactionRepository.GetAccountStatistics();
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.GetSuccesfully;
                response.Data = listAccount;
            }

            return response;
        }
    }
}
