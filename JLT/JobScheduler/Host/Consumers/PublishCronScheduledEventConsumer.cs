using JLT.JobScheduler.Host.Extensions;
using JLT.JobScheduler.Host.Jobs;
using JLT.JobScheduler.Messages.Commands;
using MassTransit;
using Quartz;

namespace JLT.JobScheduler.Host.Consumers
{
    /// <summary>
    /// PLEASE NOTE THAT THIS INVOLVES SOME CUSTOM CODE - BASED ON MASSTRANSIT - AND DOES NOT YET WORK...
    /// </summary>
    public class PublishCronScheduledEventConsumer : Consumes<IPublishCronScheduledEvent>.Context
    {
        public IScheduler Scheduler { get; set; }

        public void Consume(IConsumeContext<IPublishCronScheduledEvent> context)
        {
            Scheduler.ScheduleJob<ScheduledEventJob>(context, context.Message.CorrelationId, context.Message.Destination, context.Message.CronExpression);
        }
    }
}
