using JLT.BenefitCosts.Domain.Employees;
using JLT.Framework.Service.Data.NH.Mapping;

namespace JLT.BenefitCosts.Domain.Mappings
{
    class PolicyCostMap : EntityMapBase<PolicyCost>
    {
        public PolicyCostMap()
        {
            Map(x => x.Cost);
            Map(x => x.EligibilityGroupId);
            Map(x => x.EmployeeAmount);
            Map(x => x.EmployeeBenefitPolicyId);
            Map(x => x.EmployerAmount);
            Map(x => x.RequestedValue);
            Map(x => x.TaxableAmount);
            Map(x => x.Value);
        }
    }
}
