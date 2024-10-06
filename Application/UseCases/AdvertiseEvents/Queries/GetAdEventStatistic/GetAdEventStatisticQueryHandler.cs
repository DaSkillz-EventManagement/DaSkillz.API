using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.AdvertiseEvents.Queries.GetAdEventStatistic
{
    public class GetAdEventStatisticQueryHandler : IRequestHandler<GetAdEventStatisticQuery, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdvertisedEventRepository _advertisedEventRepository;

        public GetAdEventStatisticQueryHandler(IUserRepository userRepository, IAdvertisedEventRepository advertisedEventRepository)
        {
            _userRepository = userRepository;
            _advertisedEventRepository = advertisedEventRepository;
        }

        public async Task<APIResponse> Handle(GetAdEventStatisticQuery request, CancellationToken cancellationToken)
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
                var listAdEvent = await _advertisedEventRepository.GetAdEventStatistic();
                response.StatusResponse = HttpStatusCode.OK;
                response.Message = MessageCommon.GetSuccesfully;
                response.Data = listAdEvent;
            }
            return response;
        }
    }
}
