using JLT.BenefitCosts.Domain.Companies;
using JLT.Framework.Service.Data.NH.Mapping;

namespace JLT.BenefitCosts.Domain.Mappings.Companies
{
    public class BenefitSchemeCoverLevelMap : EntityMapBase<BenefitSchemeCoverLevel>
    {
        public BenefitSchemeCoverLevelMap()
        {
            Map(x => x.BenefitSchemeId);
            Map(x => x.CostFormula);
        }
    }
}
