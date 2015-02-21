using System;

namespace JLT.BenefitCosts.Host.Messages
{
    public class EvaluateBenefitValueResponse
    {
        public Guid EmployeeId { get; set; }

        public Guid SchemeCoverLevelId { get; set; }

        public decimal Value { get; set; }
    }
}
