using JLT.BenefitCosts.Domain.Companies;
using JLT.BenefitCosts.Domain.Employees;
using JLT.BenefitCosts.Messages.Commands;
using MassTransit;
using NHibernate;
using Topshelf.Logging;

namespace JLT.BenefitCosts.Host.Consumers
{
    public class CreateTestDataConsumer : Consumes<CreateTestData>.All
    {
        public ISessionFactory SessionFactory { get; set; }

        private static readonly LogWriter LogWriter = HostLogger.Get<CreateTestData>();

        public void Consume(CreateTestData message)
        {
            LogWriter.Info("! Creating Test Data !");

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
