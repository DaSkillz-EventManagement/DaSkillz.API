using Domain.Repositories;
using Quartz;

namespace Infrastructure.ExternalServices.Quartz
{

    public class EventStatusToOngoingJob : IJob
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IEventRepository _eventRepo;

        public EventStatusToOngoingJob(ISchedulerFactory schedulerFactory, IEventRepository eventRepo)
        {
            _schedulerFactory = schedulerFactory;
            _eventRepo = eventRepo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // get scheduler
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            // get job and trigger
            IJobDetail currentJob = context.JobDetail;
            ITrigger currentTrigger = context.Trigger;

            _eventRepo.UpdateEventStatusToOnGoing(Guid.Parse(currentJob.Key.ToString().Substring(14)));
            //_eventService.UpdateEventStatusEnded(currentJob.Key.ToString());
            Console.WriteLine("Task run: Event status changed to Ongoing!");

            // delete job and trigger
            await scheduler.DeleteJob(currentJob.Key);
            await scheduler.UnscheduleJob(currentTrigger.Key);

            Console.WriteLine($"DeleteJob Event status changed to Ongoing Job: {currentJob.Key}");
        }
    }
}
