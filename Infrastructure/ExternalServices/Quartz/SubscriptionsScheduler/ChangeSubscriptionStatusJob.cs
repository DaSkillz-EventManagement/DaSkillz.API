using Application.Abstractions.Caching;
using Application.Abstractions.Payment.ZaloPay;
using Domain.Entities;
using Domain.Repositories.UnitOfWork;
using Domain.Repositories;
using Infrastructure.ExternalServices.Quartz.PaymentScheduler;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Quartz.SubscriptionsScheduler
{
    public class ChangeSubscriptionStatusJob : IJob
    {
        private readonly ISchedulerFactory _scheduler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public ChangeSubscriptionStatusJob(ISchedulerFactory scheduler, IUnitOfWork unitOfWork, ISubscriptionRepository subscriptionRepository)
        {
            _scheduler = scheduler;
            _unitOfWork = unitOfWork;
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //get scheduler
            IScheduler scheduler = await _scheduler.GetScheduler();
            IJobDetail currentJob = context.JobDetail;

            //process scheduler
            var result = await _subscriptionRepository.GetAll();
            if (result != null)
            {
                return;
            }

            foreach (var a in result!)
            {
                if (a.EndDate > DateTime.UtcNow)
                    a.IsActive = false;

                await _subscriptionRepository.Update(a);
            }

            await _unitOfWork.SaveChangesAsync();

            

            //end job
            await scheduler.DeleteJob(currentJob.Key);

        }

    }
}
