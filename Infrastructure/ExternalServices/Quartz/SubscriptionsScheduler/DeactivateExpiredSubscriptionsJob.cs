using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Quartz;

public class DeactivateExpiredSubscriptionsJob : IJob
{
    //private readonly ILogger<DeactivateExpiredSubscriptionsJob> _logger;
    private readonly IUserRepository userRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeactivateExpiredSubscriptionsJob(IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
    {
        this.userRepository = userRepository;
        _subscriptionRepository = subscriptionRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {

        // Use a bulk update to deactivate expired subscriptions
        var updatedRows = await userRepository.UpdateIsPremiumUser();

        foreach (var user in updatedRows)
        {
            var existSubscription = await _subscriptionRepository.GetByUserId(user.UserId);
            if (existSubscription!.IsActive)
            {
                user.IsPremiumUser = true;
            }
            else
            {
                user.IsPremiumUser = false;
            }

        }

        await unitOfWork.SaveChangesAsync();
    }
}
