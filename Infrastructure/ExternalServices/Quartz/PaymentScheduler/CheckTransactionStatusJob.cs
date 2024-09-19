using Application.Abstractions.Caching;
using Application.Abstractions.Payment.ZaloPay;
using Domain.Enum.Payment;
using Domain.Repositories;
using Domain.Repositories.UnitOfWork;
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

        public CheckTransactionStatusJob(ISchedulerFactory scheduler, ITransactionRepository transactionRepository, IZaloPayService zaloPayService, IRedisCaching caching, IUnitOfWork unitOfWork)
        {
            _scheduler = scheduler;
            _transactionRepository = transactionRepository;
            _zaloPayService = zaloPayService;
            _caching = caching;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // get scheduler
            IScheduler scheduler = await _scheduler.GetScheduler();
            IJobDetail currentJob = context.JobDetail;

            var cacheKeys = _caching.SearchKeysAsync("payment");

            var tasks = cacheKeys.Select(async key =>
            {
                // Get transactionId from cache
                var transactionId = await _caching.HashGetSpecificKeyAsync(key, "transactionId");
                if (string.IsNullOrEmpty(transactionId)) return; 

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
            await scheduler.DeleteJob(currentJob.Key);
        }

    }
}
