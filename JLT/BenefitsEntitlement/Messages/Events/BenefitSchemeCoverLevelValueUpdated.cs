using System;

namespace JLT.BenefitCosts.Messages.Events
{
    public interface IEmployeeBenefitValueUpdated
    {
        Guid EmployeeId { get; }
        Guid BenefitSchemeId { get; }
        Guid BenefitSchemeCoverLevelId { get; }
        decimal PreviousValue { get; }
        decimal NewValue { get; }
    }
}
