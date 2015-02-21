using JLT.BenefitCosts.Domain.Employees;
using JLT.Framework.Service.Data.NH.Mapping;
using FluentNHibernate.Mapping;

namespace JLT.BenefitCosts.Domain.Mappings.Employees
{
    public class EmployeeBenefitCoverLevelMap : EntityMapBase<EmployeeBenefitCoverLevel>
    {
        public EmployeeBenefitCoverLevelMap()
        {
            Map(x => x.BenefitSchemeId);
            Map(x => x.CoverLevelId);
            Map(x => x.LatestCost).Access.CamelCaseField(Prefix.Underscore);
        }
    }
}
