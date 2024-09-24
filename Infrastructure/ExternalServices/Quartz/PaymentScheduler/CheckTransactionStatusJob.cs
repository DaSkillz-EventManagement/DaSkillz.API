using Application.Abstractions.Caching;
using Application.Abstractions.Payment.ZaloPay;
using Domain.Entities;
using Domain.Enum.Payment;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.ExternalServices.Quartz.PaymentScheduler
{
    public class CheckTransactionStatusJob : IJob
    {
        private readonly ISchedulerFactory _scheduler;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IZaloPayService _zaloPayService;
        private readonly IRedisCaching _caching;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CheckTransactionStatusJob> _logger;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public CheckTransactionStatusJob(ISchedulerFactory scheduler,
            ITransactionRepository transactionRepository,
            IZaloPayService zaloPayService,
            IRedisCaching caching,
            IUnitOfWork unitOfWork,
            ILogger<CheckTransactionStatusJob> logger,
            ISubscriptionRepository subscriptionRepository)
        {
            _scheduler = scheduler;
            _transactionRepository = transactionRepository;
            _zaloPayService = zaloPayService;
            _caching = caching;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //get scheduler
            IScheduler scheduler = await _scheduler.GetScheduler();
            IJobDetail currentJob = context.JobDetail;
            List<string> cacheKeys = new List<string>();
            try
            {
                cacheKeys = await _caching.SearchKeysAsync("payment");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get keys from Redis. Fallback to database.");
                var processingTransaction = await _transactionRepository.getProcessingTransaction();
                cacheKeys = (List<string>)processingTransaction;
            }

            //if (!cacheKeys.Any())
            //{
            //    var processingTransaction = await _transactionRepository.getProcessingTransaction();
            //    cacheKeys = (List<string>)processingTransaction;
            //}

            var tasks = cacheKeys.Select(async key =>
            {
                // Get transactionId from cache
                //var transactionId = await _caching.HashGetSpecificKeyAsync(key, "transactionId");

                string? transactionId = null;
                try
                {
                    transactionId = await _caching.HashGetSpecificKeyAsync(key, "transactionId");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to get transactionId from Redis for key {key}.", key);
                }

                if (string.IsNullOrEmpty(transactionId))
                {
                    var existInDb = await _transactionRepository.GetById(key);
                    if (existInDb == null || existInDb.Status != (int)TransactionStatus.PROCESSING)
                        return;

                    transactionId = existInDb.Apptransid;
                }

                // Query ZaloPay API
                var result = await _zaloPayService.QueryOrderStatus(transactionId);
                if (result == null || !result.ContainsKey("return_code")) return;

                var exist = await _transactionRepository.GetById(transactionId);
                if (exist == null) return;

                var returnCode = int.Parse(result["return_code"].ToString()!);
                switch (returnCode)
                {
                    case 1:
                        exist.Zptransid = result["zp_trans_id"].ToString();
                        exist.Status = (int)TransactionStatus.SUCCESS;
                        await _transactionRepository.Update(exist);
                        if (exist.IsSubscription)
                        {


                            var subscription = await _subscriptionRepository.GetByUserId(exist.UserId);
                            if (subscription != null)
                            {
                                if (subscription.EndDate >= DateTime.UtcNow)
                                {
                                    subscription.EndDate = subscription.EndDate.AddMonths(1);
                                }
                                else
                                {
                                    subscription.StartDate = DateTime.UtcNow;
                                    subscription.EndDate = DateTime.UtcNow.AddMonths(1);
                                }
                                subscription.IsActive = true;
                            }
                            else
                            {
                                subscription = new Subscription
                                {
                                    UserId = (Guid)exist.UserId!,
                                    StartDate = DateTime.UtcNow,
                                    EndDate = DateTime.UtcNow.AddMonths(1),
                                    IsActive = true
                                };
                                await _subscriptionRepository.Add(subscription);
                            }
                        }


                        await _unitOfWork.SaveChangesAsync();
                        await _caching.RemoveAsync(key);
                        break;

                    case 2:
                        exist.Zptransid = result["zp_trans_id"].ToString();
                        exist.Status = (int)TransactionStatus.FAIL;
                        await _transactionRepository.Update(exist);
                        await _caching.RemoveAsync(key);
                        break;
                }

                await _unitOfWork.SaveChangesAsync();
            });



            await Task.WhenAll(tasks);

            // Xóa job sau khi hoàn thành
            //await scheduler.DeleteJob(currentJob.Key);
        }

    }


}

