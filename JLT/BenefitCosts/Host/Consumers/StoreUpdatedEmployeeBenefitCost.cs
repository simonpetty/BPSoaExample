using JLT.BenefitCosts.Domain.Employees;
using JLT.OnlineBenefits.Messages.RequestResponse;
using MassTransit;
using NHibernate;

namespace JLT.BenefitCosts.Host.Consumers
{
    public class StoreUpdatedEmployeeBenefitCost : Consumes<EvaluateBenefitCostResponse>.All
    {
        public IServiceBus Bus { get; set; }
        public ISessionFactory SessionFactory { get; set; }

        public void Consume(EvaluateBenefitCostResponse message)
        {
            using (var s = SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                var employee = s.Load<Employee>(message.EmployeeId);
                employee.SetBenefitCoverLevelCost(message.BenefitSchemeId, message.CoverLevelId, message.Cost);
                tx.Commit();
            }
        }
    }
}
