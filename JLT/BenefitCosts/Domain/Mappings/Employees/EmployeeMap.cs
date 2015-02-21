using JLT.BenefitCosts.Domain.Employees;
using JLT.Framework.Service.Data.NH.Mapping;

namespace JLT.BenefitCosts.Domain.Mappings.Employees
{
    public class EmployeeMap : EntityMapBase<Employee>
    {
        public EmployeeMap()
        {
            Map(x => x.CompanyId);
            HasMany(x => x.EmployeeBenefitCoverLevels).LazyLoad().AsBag();
        }
    }
}
