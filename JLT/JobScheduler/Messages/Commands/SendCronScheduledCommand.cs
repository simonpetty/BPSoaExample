using System;
using System.Collections.Generic;
using System.Linq;
using MassTransit;
using Quartz;

namespace JLT.JobScheduler.Messages.Commands
{
    public class SendCronScheduledCommand<T> : ISendCronScheduledCommand<T> where T : class
    {
        public SendCronScheduledCommand(CronScheduleBuilder scheduleBuilder, Uri destination, T payload)
        {
            CorrelationId = NewId.NextGuid();
            CronExpression = ((ICronTrigger) scheduleBuilder.Build()).CronExpressionString;
            Destination = destination;
            Payload = payload;
            PayloadType = typeof(T).GetMessageTypes()
                .Select(x => new MessageUrn(x).ToString())
                .ToList();
        }

        public Guid CorrelationId { get; private set; }
        public string CronExpression { get; private set; }
        public IList<string> PayloadType { get; private set; }
        public Uri Destination { get; private set; }
        public T Payload { get; private set; }
    }
}
