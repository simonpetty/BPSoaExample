using System;
using System.Collections.Generic;
using JLT.BenefitCosts.Messages.Commands;
using JLT.OnlineBenefits.Messages.Events;
using MassTransit;

namespace JLT.OnlineBenefitsSimulator
{
    class Program
    {
        /// <summary>
        /// intialized the bus to use rabbitmq.
        /// </summary>
        private static readonly IServiceBus Bus =
            ServiceBusFactory.New(sbc =>
            {
                sbc.UseRabbitMq();
                sbc.UseJsonSerializer();
                sbc.ReceiveFrom("rabbitmq://localhost/JLT.OnlineBenefitsSimulator");
            });

        internal class EmployeeGradeChanged : IEmployeeGradeChanged
        {
            public Guid CompanyId { get; set; }
            public DateTime EffectiveDateValue { get; set; }
            public string NewValue { get; set; }
            public string OldValue { get; set; }
            public Guid EmployeeId { get; set; }
            public string NamedVariable { get; set; }
        }

        internal class ProfileCreated : IProfileCreated
        {
            public Guid Id { get; set; }
            public string EligibilityFormula { get; set; }
            public List<Guid> PensionSchemeIds { get; set; }
        }

        internal static void StartPensionEligibilityMessages()
        {
            Console.WriteLine("Press enter to create pensions eligibility test data...");
            Console.ReadLine();

            var employeeId = new Guid("492EB071-6419-4107-A8A9-7D52235FC232");

            Bus.Publish(new EmployeeCreated
            {
                EmployeeId = employeeId
            });

            var profileId = Guid.NewGuid();

            Console.WriteLine("Press enter to create profile ($EmployeeGrade=\"B\")...");
            Console.ReadLine();

            Bus.Publish(new ProfileCreated
            {
                Id = profileId,
                EligibilityFormula = "$EmployeeGrade=\"B\"",
                PensionSchemeIds = new List<Guid>(new[] { Guid.NewGuid() })
            });

            Console.WriteLine("Press enter to change grade \"B\"...");
            Console.ReadLine();

            Bus.Publish(new EmployeeGradeChanged
            {
                EmployeeId = employeeId,
                OldValue = "A",
                NewValue = "B"
            });
        }

        internal static void StartBenefitCostsMessages()
        {
            Console.WriteLine("Press enter to create benefit costs test data...");
            Console.ReadLine();

            var employeeId = Guid.NewGuid();
            var companyId = Guid.NewGuid();

            Bus.Publish(new CreateTestData
            {
                EmployeeId = employeeId,
                CompanyId = companyId,
                BenefitSchemeId = Guid.NewGuid(),
                CoverLevelId = Guid.NewGuid(),
                CostFormula = "$EmployeeGrade = \"B\""
            }, x =>
            {
                x.SetHeader("JLT.ContextIdentity", "Simon Petty");
                x.SetHeader("JLT.ClaimId", "1234");
            });

            Console.WriteLine("Press enter to trigger benefit cost re-evaluation...");
            Console.ReadLine();

            Bus.Publish(new EmployeeGradeChanged
            {
                EmployeeId = employeeId,
                OldValue = "A",
                NewValue = "B",
                NamedVariable = "EmployeeGrade",
                CompanyId = companyId
            }, x =>
            {
                x.SetHeader("JLT.ContextIdentity", "Simon Petty");
                x.SetHeader("JLT.ClaimId", "1234");
            });
        }

        internal static void StartScheduledCommandTestMessages()
        {
            Console.WriteLine("Press enter to publish employee created event...");
            Console.ReadLine();

            Bus.Publish(new EmployeeCreated
            {
                CompanyId = Guid.NewGuid(),
                //ContinuousEmploymentStartDate = DateTime.Today,
                EmployeeId = Guid.NewGuid()
            }, x =>
            {
                x.SetHeader("JLT.ContextIdentity", "Simon Petty");
                x.SetHeader("JLT.ClaimId", "1234");
            });
        }

        static void Main(string[] args)
        {
            //StartPensionEligibilityMessages();

            //StartBenefitCostsMessages();

            //StartScheduledCommandTestMessages();

            Console.WriteLine("Press modify code to decide what test messages to send");
        }

        
    }
}
