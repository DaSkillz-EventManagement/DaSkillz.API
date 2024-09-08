using Domain.Repositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Quartz
{
    public class EventStatusToEndedJob : IJob
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IEventRepository _eventRepository;

        public EventStatusToEndedJob(ISchedulerFactory schedulerFactory, IEventRepository eventRepository)
        {
            _schedulerFactory = schedulerFactory;
            _eventRepository = eventRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // get scheduler
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            // get job and trigger
            IJobDetail currentJob = context.JobDetail;
            ITrigger currentTrigger = context.Trigger;
            _eventRepository.UpdateEventStatusEnded(Guid.Parse(currentJob.Key.ToString().Substring(14)));
            Console.WriteLine("Task run: Event status changed to Ended!");

            // delete job and trigger
            await scheduler.DeleteJob(currentJob.Key);
            await scheduler.UnscheduleJob(currentTrigger.Key);

            Console.WriteLine($"DeleteJob Event status changed to Ended Job: {currentJob.Key}");
            //return Task.CompletedTask;
        }
    }
}
