using System;
using System.Collections.Generic;
using MassTransit;

namespace JLT.JobScheduler.Messages.Commands
{
    public interface IPublishCronScheduledEvent<out T> : IPublishCronScheduledEvent where T : class
    {
        T Payload { get; }
    }

    public interface IPublishCronScheduledEvent : CorrelatedBy<Guid>
    {
        string CronExpression { get; }
        IList<string> PayloadType { get; }
        Uri Destination { get; }
    }
}
