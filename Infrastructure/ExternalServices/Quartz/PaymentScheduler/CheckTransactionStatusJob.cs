﻿using Application.Abstractions.Caching;
using Application.Abstractions.Payment.ZaloPay;
using Application.UseCases.Payment.Queries.GetOrderStatus;
using Domain.Enum.Payment;
using Domain.Repositories;
using MediatR;
using Quartz;

namespace Infrastructure.ExternalServices.Quartz.PaymentScheduler
{
    public class CheckTransactionStatusJob : IJob
    {
        private readonly ISchedulerFactory _scheduler;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IZaloPayService _zaloPayService;
        private readonly IRedisCaching _caching;
        private readonly IMediator _mediator;

        public CheckTransactionStatusJob(ISchedulerFactory scheduler, ITransactionRepository transactionRepository, IZaloPayService zaloPayService, IRedisCaching caching, IMediator mediator)
        {
            _scheduler = scheduler;
            _transactionRepository = transactionRepository;
            _zaloPayService = zaloPayService;
            _caching = caching;
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //get scheduler
            IScheduler scheduler = await _scheduler.GetScheduler();
            IJobDetail currentJob = context.JobDetail;
            List<string> cacheKeys = new List<string>();

            cacheKeys = await _caching.SearchKeysAsync("payment");
            if (cacheKeys == null)
            {
                var processingTransaction = await _transactionRepository.getProcessingTransaction();
                cacheKeys = (List<string>)processingTransaction;
            }


            var tasks = cacheKeys.Select(async key =>
            {
                // Get transactionId from cache

                string? transactionId = null;

                transactionId = await _caching.HashGetSpecificKeyAsync(key, "transactionId");

                if (string.IsNullOrEmpty(transactionId))
                {
                    var existInDb = await _transactionRepository.GetById(key);
                    if (existInDb == null || existInDb.Status != (int)TransactionStatus.PROCESSING)
                        return;

                    transactionId = existInDb.Apptransid;
                }

                var result = await _zaloPayService.QueryOrderStatus(transactionId);

                // Query ZaloPay API
                var queryOrder = await _mediator.Send(new GetOrderStatusQuery(transactionId));
                if (queryOrder.Data == null) return;

                var exist = await _transactionRepository.GetById(transactionId);
                if (exist == null) return;


            });



            await Task.WhenAll(tasks);

            // Xóa job sau khi hoàn thành
            //await scheduler.DeleteJob(currentJob.Key);
        }

    }


}

