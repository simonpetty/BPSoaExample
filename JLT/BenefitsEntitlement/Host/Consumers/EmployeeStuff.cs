using System;
using JLT.BenefitCosts.Domain.Companies;
using JLT.BenefitCosts.Domain.Employees;
using JLT.BenefitCosts.Host.Messages;
using JLT.BenefitCosts.Messages.Events;
using JLT.JobScheduler.Messages.Commands;
using JLT.OnlineBenefits.Messages.Events;
using MassTransit;
using NHibernate;
using Quartz;

namespace JLT.BenefitCosts.Host.Consumers
{
    public class EmployeeStuff : 
        Consumes<EmployeeCreated>.All,
        Consumes<IFormulaTokenValueChanged>.All, 
        Consumes<EvaluateBenefitCostResponse>.All
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
            Bus.GetEndpoint(new Uri("rabbitmq://localhost/JLT.JobScheduler")).Send(command);
        }

        public void Consume(IFormulaTokenValueChanged message)
        {
            using (var s = SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                var company = s.Load<Company>(message.CompanyId);
                foreach (var cl in company.BenefitSchemeCoverLevels)
                {
                    if (cl.CostFormula.Contains(message.NamedVariable))
                        EvaluateEmployeeBenefitCost(message.EmployeeId, cl);

                    if (cl.ValueFormula.Contains(message.NamedVariable))
                        EvaluateEmployeeBenefitValue(message.EmployeeId, cl);
                }
                tx.Commit();
            }
        }

        public void Consume(EvaluateBenefitCostResponse message)
        {
            using (var s = SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                var employee = s.Load<Employee>(message.EmployeeId);
                employee.SetBenefitCoverLevelCost(message.SchemeCoverLevelId, message.Value);
                tx.Commit();
            }
        }

        public void Consume(EvaluateBenefitValueResponse message)
        {
            using (var s = SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                var employee = s.Load<Employee>(message.EmployeeId);
                employee.SetBenefitCoverLevelValue(message.SchemeCoverLevelId, message.Value);
                tx.Commit();
            }
        }

        private void EvaluateEmployeeBenefitValue(Guid employeeId, BenefitSchemeCoverLevel coverLevel)
        {
            Bus.Endpoint.Send(new EvaluateBenefitValueRequest
                {
                    EmployeeId = employeeId,
                    BenefitRateFormula = coverLevel.ValueFormula,
                    SchemeCoverLevelId = coverLevel.Id,
                });
        }

        private void EvaluateEmployeeBenefitCost(Guid employeeId, BenefitSchemeCoverLevel coverLevel)
        {
            Bus.Endpoint.Send(new EvaluateBenefitCostRequest
                {
                    EmployeeId = employeeId,
                    BenefitRateFormula = coverLevel.CostFormula,
                    SchemeCoverLevelId = coverLevel.Id,
                });
        }
    }
}
