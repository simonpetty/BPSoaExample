using System;
using JLT.JobScheduler.Messages.Commands;
using JLT.OnlineBenefits.Messages.Events;
using JLT.PensionsEntitlement.Messages.Events;
using MassTransit;
using Quartz;
using Topshelf.Logging;

namespace JLT.PensionsEntitlement.Host.Consumers
{
    public class ScheduleCommandOnEmployeeCreated : Consumes<EmployeeCreated>.All
    {
        public IServiceBus Bus { get; set; }

        private static readonly LogWriter LogWriter = HostLogger.Get<ScheduleCommandOnEmployeeCreated>();

        public void Consume(EmployeeCreated message)
        {
            LogWriter.Info("! Scheduling EmployeeStartedEmployment event to be sent back here in one minutes time !");

            var schedule = CronScheduleBuilder.DailyAtHourAndMinute(DateTime.Now.Hour, DateTime.Now.Minute + 1);

            var futureCommand = new EmployeeStartedEmployment
                {
                    EmployeeId = message.EmployeeId,
                    CompanyId = message.CompanyId
                };

            var command = new SendCronScheduledCommand<EmployeeStartedEmployment>(schedule, Bus.Endpoint.Address.Uri, futureCommand);
            //Bus.GetEndpoint(typeof(command)).Send(command); <- why doesn't this work?
            Bus.GetEndpoint(new Uri("rabbitmq://localhost/JLT.JobScheduler.Host")).Send(command);
        }
    }
}
