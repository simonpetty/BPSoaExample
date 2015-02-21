using JLT.BenefitCosts.Domain.Companies;
using JLT.Framework.Service.Data.NH.Mapping;

namespace JLT.BenefitCosts.Domain.Mappings
{
    public class BenefitSchemeCoverLevelMap : EntityMapBase<BenefitSchemeCoverLevel>
    {
        public BenefitSchemeCoverLevelMap()
        {
            Map(x => x.BenefitSchemeId);
            Map(x => x.ValueFormula);
            Map(x => x.CostFormula);
        }
    }
}
