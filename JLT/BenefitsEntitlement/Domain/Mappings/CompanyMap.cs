using JLT.BenefitCosts.Domain.Companies;
using JLT.Framework.Service.Data.NH.Mapping;

namespace JLT.BenefitCosts.Domain.Mappings
{
    public class CompanyMap : EntityMapBase<Company>
    {
        public CompanyMap()
        {
            HasMany(x => x.BenefitSchemeCoverLevels).LazyLoad().AsBag();
        }
    }
}
