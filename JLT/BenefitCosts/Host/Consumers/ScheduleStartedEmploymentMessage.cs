using System;
using JLT.BenefitCosts.Messages.Events;
using JLT.JobScheduler.Messages.Commands;
using JLT.OnlineBenefits.Messages.Events;
using MassTransit;
using NHibernate;
using Quartz;

namespace JLT.BenefitCosts.Host.Consumers
{
    public class ScheduleStartedEmploymentMessage : Consumes<EmployeeCreated>.All
    {
        public IServiceBus Bus { get; set; }
        public ISessionFactory SessionFactory { get; set; }

        public void Consume(EmployeeCreated message)
        {
            var schedule = CronScheduleBuilder.DailyAtHourAndMinute(14, 14);

            var futureCommand = new EmployeeStartedEmployment
                {
                    EmployeeId = message.EmployeeId,
                    CompanyId = message.CompanyId
                };

            var command = new PublishCronScheduledEvent<EmployeeStartedEmployment>(schedule, Bus.Endpoint.Address.Uri, futureCommand);
            //Bus.Endpoint.Send(command); <- why doesn't this work?
            Bus.GetEndpoint(new Uri("rabbitmq://localhost/JLT.JobScheduler")).Send(command);
        }
    }
}
