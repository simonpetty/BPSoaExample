using JLT.BenefitCosts.Domain.Companies;
using JLT.BenefitCosts.Domain.Employees;
using JLT.BenefitCosts.Messages.Events;
using MassTransit;
using NHibernate;
using Topshelf.Logging;

namespace JLT.BenefitCosts.Host.Consumers
{
    public class CreateTestData : Consumes<CreateStuff>.All
    {
        public ISessionFactory SessionFactory { get; set; }

        private static readonly LogWriter LogWriter = HostLogger.Get<CreateTestData>();

        public void Consume(CreateStuff message)
        {
            LogWriter.Info("This is an example of a log entry that will be written to console output");

            using (var s = SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                var company = new Company(message.CompanyId);
                company.AddBenefitSchemeCoverLevel(new BenefitSchemeCoverLevel(message.CoverLevelId)
                    {
                        BenefitSchemeId = message.BenefitSchemeId,
                        CostFormula = message.CostFormula
                    });

                var employee = new Employee(message.EmployeeId);
                s.Save(employee);

                s.Save(company);
                tx.Commit();
            }
        }
    }
}
