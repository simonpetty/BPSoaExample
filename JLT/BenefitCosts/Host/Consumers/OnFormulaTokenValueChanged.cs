using JLT.BenefitCosts.Domain.Companies;
using JLT.OnlineBenefits.Messages.Events;
using JLT.OnlineBenefits.Messages.RequestResponse;
using MassTransit;
using NHibernate;

namespace JLT.BenefitCosts.Host.Consumers
{
    public class OnFormulaTokenValueChanged : Consumes<IFormulaTokenValueChanged>.All
    {
        public IServiceBus Bus { get; set; }
        public ISessionFactory SessionFactory { get; set; }

        public void Consume(IFormulaTokenValueChanged message)
        {
            using (var s = SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                var company = s.Load<Company>(message.CompanyId);
                foreach (var coverLevel in company.BenefitSchemeCoverLevels)
                {
                    if (coverLevel.CostFormula.Contains(message.NamedVariable))
                    {
                        Bus.GetEndpoint(typeof(EvaluateBenefitCostRequest)).Send(new EvaluateBenefitCostRequest
                        {
                            EmployeeId = message.EmployeeId,
                            CostFormula = coverLevel.CostFormula,
                            BenefitSchemeId = coverLevel.BenefitSchemeId,
                            CoverLevelId = coverLevel.Id,
                        });
                    }

                }
                tx.Commit();
            }
        }
    }
}
