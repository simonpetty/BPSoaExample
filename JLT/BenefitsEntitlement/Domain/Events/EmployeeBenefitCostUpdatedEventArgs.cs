using System;
using JLT.BenefitCosts.Messages.Events;

namespace JLT.BenefitCosts.Domain.Events
{
    public class EmployeeBenefitCostUpdatedEventArgs : EventArgs, IEmployeeBenefitCostUpdated
    {
        public Guid EmployeeId { get; set; }

        public Guid BenefitSchemeId { get; set; }

        public Guid BenefitSchemeCoverLevelId { get; set; }

        public decimal? PreviousCost { get; set; }

        public decimal NewCost { get; set; }
    }
}
