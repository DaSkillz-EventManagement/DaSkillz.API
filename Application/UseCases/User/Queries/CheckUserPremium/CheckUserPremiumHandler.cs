using Application.ResponseMessage;
using Domain.Models.Response;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.User.Queries.CheckUserPremium
{
    public class CheckUserPremiumHandler : IRequestHandler<CheckUserPremiumQuery, APIResponse>
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CheckUserPremiumHandler(ISubscriptionRepository subscriptionRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(CheckUserPremiumQuery request, CancellationToken cancellationToken)
        {
            var update = await _subscriptionRepository.UpdateExpiredSubscription(request.userId);
            var exist = await _subscriptionRepository.GetByUserId(request.userId);
            var user = await _userRepository.GetById(request.userId);
            if (user == null)
            {
                return new APIResponse
                {
                    StatusResponse = HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                };
            }
            user.IsPremiumUser = exist?.IsActive ?? false;
            await _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.Complete,
                Data = new { userId = request.userId, isPremium = exist?.IsActive ?? false }, 
            };
        }
    }
}
