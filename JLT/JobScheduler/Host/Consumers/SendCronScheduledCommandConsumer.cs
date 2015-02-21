using JLT.JobScheduler.Host.Extensions;
using JLT.JobScheduler.Messages.Commands;
using MassTransit;
using MassTransit.QuartzIntegration;
using Quartz;

namespace JLT.JobScheduler.Host.Consumers
{
    public class SendCronScheduledCommandConsumer : Consumes<ISendCronScheduledCommand>.Context
    {
        public IScheduler Scheduler { get; set; }

        public void Consume(IConsumeContext<ISendCronScheduledCommand> context)
        {
            Scheduler.ScheduleJob<ScheduledMessageJob>(context, context.Message.CorrelationId, context.Message.Destination, context.Message.CronExpression);
        }
    }
}
