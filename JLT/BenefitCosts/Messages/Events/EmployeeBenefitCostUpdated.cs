using System;

namespace JLT.BenefitCosts.Messages.Events
{
    public interface IEmployeeBenefitCostUpdated
    {
        Guid EmployeeId { get; }
        Guid BenefitSchemeId { get; }
        Guid BenefitSchemeCoverLevelId { get; }
        decimal? PreviousCost { get; }
        decimal NewCost { get; }
    }
}
