using JLT.BenefitCosts.Domain.Employees;
using JLT.BenefitCosts.Messages;
using MassTransit;
using NHibernate;

namespace JLT.BenefitCosts.Host.Consumers
{
    public class CreatePolicyCost : Consumes<CreatedPolicyCost>.All
    {
        public ISessionFactory SessionFactory { get; set; }

        public void Consume(CreatedPolicyCost message)
        {
            using(var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var policyCost = new PolicyCost
                    {
                        Id = message.Id,
                        Cost = message.Cost,
                        EligibilityGroupId = message.EligibilityGroupId,
                        EmployeeAmount = message.EmployeeAmount,
                        EmployeeBenefitPolicyId = message.EmployeeBenefitPolicyId,
                        EmployerAmount = message.EmployerAmount,
                        TaxableAmount = message.TaxableAmount,
                        Value = message.Value,
                        RequestedValue = message.RequestedValue
                    };

                session.Save(policyCost);
                tx.Commit();
            }
        }
    }
}
