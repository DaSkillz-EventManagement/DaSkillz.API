using Domain.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Quartz
{
    public class AllEventStatusToOngoingJob : IJob
    {
        private readonly IEventRepository _eventRepository;
        private readonly ISchedulerFactory _schedulerFactory;

        public AllEventStatusToOngoingJob(IEventRepository eventRepository, ISchedulerFactory schedulerFactory)
        {
            _eventRepository = eventRepository;
            _schedulerFactory = schedulerFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // get scheduler
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            // get job and trigger
            IJobDetail currentJob = context.JobDetail;
            ITrigger currentTrigger = context.Trigger;

            _eventRepository.UpdateEventStatusToOnGoing();
            _eventRepository.UpdateEventStatusToEnded();
            Console.WriteLine("Task run: Event status changed to Ongoing!");

            // delete job and trigger
            await scheduler.DeleteJob(currentJob.Key);
            await scheduler.UnscheduleJob(currentTrigger.Key);

            Console.WriteLine($"DeleteJob Event status changed to Ongoing Job: {currentJob.Key}");
        }
    }
}
