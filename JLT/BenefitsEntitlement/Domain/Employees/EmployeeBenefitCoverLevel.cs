using JLT.BenefitCosts.Domain.Companies;

namespace JLT.BenefitCosts.Domain.Employees
{
    public class EmployeeBenefitCoverLevel
    {
        public virtual BenefitSchemeCoverLevel SchemeCoverLevel { get; set; }

        public virtual decimal? Cost { get; set; }

        public virtual decimal? Value { get; set; }
    }
}
