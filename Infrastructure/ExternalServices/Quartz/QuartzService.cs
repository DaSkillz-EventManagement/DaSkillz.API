using Application.ExternalServices.Quartz;
using Application.Helper;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.Quartz
{
    public class QuartzService : IQuartzService
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public QuartzService(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public Task DeleteJobsByEventId(string eventId)
        {
            throw new NotImplementedException();
        }

        public async Task ExcuteJob(string key, int type)
        {
            var eventId = Guid.Parse(key);
            switch (type)
            {
                case 1:
                    {
                        await StartEventStatusToOngoingJob(eventId, DateTime.Now.AddMinutes(1));
                        break;
                    }
                case 2:
                    {
                        await StartEventStatusToEndedJob(eventId, DateTime.Now.AddMinutes(1));
                        break;
                    }
                case 3:
                    {
                        await StartEventStartingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
                        break;
                    }
                case 4:
                    {
                        await StartEventEndingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
                        break;
                    }
                case 5:
                    {
                        await StartEventStatusToOngoingJob(eventId, DateTime.Now.AddMinutes(1));
                        await StartEventStatusToOngoingJob(eventId, DateTime.Now.AddMinutes(1));
                        await StartEventStartingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
                        await StartEventEndingEmailNoticeJob(eventId, DateTime.Now.AddMinutes(1));
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public async Task<IEnumerable<string>> GetAllJob()
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            var allJobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            return allJobKeys.Select(jobKey => jobKey.Name);
        }

        public Task StartEventEndingEmailNoticeJob(Guid eventId, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public Task StartEventStartingEmailNoticeJob(Guid eventId, DateTime startTime)
        {
            throw new NotImplementedException();
        }

        public async Task StartEventStatusToEndedJob(Guid eventId, DateTime startTime)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey("ended-" + eventId);
            IJobDetail job = JobBuilder.Create<EventStatusToEndedJob>()
            .WithIdentity(jobKey)
            .Build();
            var newTrigger =
                TriggerBuilder.Create().ForJob(jobKey)
                .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(startTime.AddMinutes(1))))
                .Build();
            await scheduler.ScheduleJob(job, newTrigger);
            Console.WriteLine($"ScheduleJob: Event status changed to Ended with id {jobKey}");
        }

        public async Task StartEventStatusToOngoingJob(Guid eventId, DateTime startTime)
        {
            var jobKey = new JobKey("start-" + eventId);
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            IJobDetail job = JobBuilder.Create<EventStatusToOngoingJob>()
            .WithIdentity(jobKey)
            .Build();
            /*var triggers = await scheduler.GetTriggersOfJob(jobKey);
            var trigger = triggers.ElementAt(0);*/
            var newTrigger = //trigger.GetTriggerBuilder()
                TriggerBuilder.Create().ForJob(jobKey)
                .WithSchedule(CronScheduleBuilder.CronSchedule(DateTimeHelper.GetCronExpression(startTime.AddMinutes(1))))
                .Build();
            //await scheduler.ScheduleJob(trigger.Key, newTrigger);
            await scheduler.ScheduleJob(job, newTrigger);
            Console.WriteLine($"ScheduleJob: Event status changed to Ongoing with id {jobKey}");
            //await scheduler.TriggerJob(jobKey);
        }
    }
}
