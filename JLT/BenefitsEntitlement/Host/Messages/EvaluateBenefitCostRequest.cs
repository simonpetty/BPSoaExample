using System;

namespace JLT.BenefitCosts.Host.Messages
{
    public class EvaluateBenefitCostRequest
    {
        public DateTime PolicyStartDate { get; set; }

        public string BenefitRateFormula { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid SchemeCoverLevelId { get; set; }
    }
}
