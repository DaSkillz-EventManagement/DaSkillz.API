using Application.ResponseMessage;
using Application.UseCases.User.Queries.GetAccountStatistic;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.User.Queries.GetAccountPreByEvent
{
    public class GetAccountPreByEventQueryHandler : IRequestHandler<GetAccountPreByEventQuery, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        public GetAccountPreByEventQueryHandler(ITransactionRepository transactionRepository, IUserRepository userRepository, IEventRepository eventRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public async Task<APIResponse> Handle(GetAccountPreByEventQuery request, CancellationToken cancellationToken)
        {
            var response = new APIResponse();
            var isAdmin = await _userRepository.IsAdmin(request.UserId);
            var isOwner = await _eventRepository.IsOwner(request.EventId, request.UserId);
            if (!isOwner || !isAdmin)
            {

                response.StatusResponse = HttpStatusCode.BadRequest;
                response.Message = MessageEvent.UserNotAllowToSee;
                response.Data = null;
                return response;
            }

            var listAcc = await _transactionRepository.GetAccountStatisticInfoByEventId(request.EventId);

            if (listAcc != null)
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.GetSuccesfully;
                response.Data = listAcc;
            }
            else
            {
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.GetSuccesfully;
                response.Data = null;
            }

            return response;

        }
    }
}
