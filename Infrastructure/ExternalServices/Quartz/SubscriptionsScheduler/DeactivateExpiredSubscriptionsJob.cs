using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Quartz;

public class DeactivateExpiredSubscriptionsJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DeactivateExpiredSubscriptionsJob> _logger;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public DeactivateExpiredSubscriptionsJob(ApplicationDbContext dbContext, ILogger<DeactivateExpiredSubscriptionsJob> logger, ISubscriptionRepository subscriptionRepository)
    {
        _dbContext = dbContext;
        _logger = logger;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        

            var now = DateTime.UtcNow;

        // Use a bulk update to deactivate expired subscriptions
        int updatedRows = await _subscriptionRepository.UpdateExpiredSubscription();

       _logger.LogInformation("Deactivated {count} expired subscriptions", updatedRows);
    }
}
