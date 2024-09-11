using Application.ExternalServices.Mail;
using Quartz;

namespace Infrastructure.ExternalServices.Quartz
{
    public class EventStartingEmailJob : IJob
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ISendMailTask _sendMailTask;

        public EventStartingEmailJob(ISchedulerFactory schedulerFactory, ISendMailTask sendMailTask)
        {
            _schedulerFactory = schedulerFactory;
            _sendMailTask = sendMailTask;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // get scheduler
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            // get job and trigger
            IJobDetail currentJob = context.JobDetail;
            ITrigger currentTrigger = context.Trigger;

            // call send mail task
            Console.WriteLine(currentJob.Key.ToString());
            _sendMailTask.SendMailReminder(Guid.Parse(currentJob.Key.ToString().Substring(16)));

            Console.WriteLine("Task run: Sending Event starting notice email!");

            // delete job and trigger
            await scheduler.DeleteJob(currentJob.Key);
            await scheduler.UnscheduleJob(currentTrigger.Key);

            Console.WriteLine($"DeleteJob Event starting notice email Job: {currentJob.Key}");
            //return Task.CompletedTask;
        }
    }
}
