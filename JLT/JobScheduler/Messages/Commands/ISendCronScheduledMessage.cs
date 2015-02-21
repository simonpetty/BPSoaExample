using System;
using System.Collections.Generic;
using MassTransit;

namespace JLT.JobScheduler.Messages.Commands
{
    public interface ISendCronScheduledCommand<out T> : ISendCronScheduledCommand where T : class
    {
        T Payload { get; }
    }

    public interface ISendCronScheduledCommand : CorrelatedBy<Guid>
    {
        string CronExpression { get; }
        IList<string> PayloadType { get; }
        Uri Destination { get; }
    }
}
