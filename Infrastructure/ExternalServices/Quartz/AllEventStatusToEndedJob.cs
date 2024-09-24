using Domain.Repositories;
using Quartz;

namespace Infrastructure.ExternalServices.Quartz
{
    public class AllEventStatusToEndedJob : IJob
    {
        private readonly IEventRepository _eventRepository;
        private readonly ISchedulerFactory _schedulerFactory;

        public AllEventStatusToEndedJob(IEventRepository eventRepository, ISchedulerFactory schedulerFactory)
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
            _eventRepository.UpdateEventStatusToEnded();
            Console.WriteLine("Task run: Event status changed to Ended!");

            // delete job and trigger
            await scheduler.DeleteJob(currentJob.Key);
            await scheduler.UnscheduleJob(currentTrigger.Key);

            Console.WriteLine($"DeleteJob Event status changed to Ended Job: {currentJob.Key}");
            //return Task.CompletedTask;
        }
    }
}
