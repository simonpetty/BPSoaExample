using FluentNHibernate.Mapping;
using JLT.BenefitCosts.Domain.Companies;
using JLT.Framework.Service.Data.NH.Mapping;

namespace JLT.BenefitCosts.Domain.Mappings.Companies
{
    public class CompanyMap : EntityMapBase<Company>
    {
        public CompanyMap()
        {
            HasMany(x => x.BenefitSchemeCoverLevels)
                .Access.CamelCaseField(Prefix.Underscore)
                .Cascade.AllDeleteOrphan()
                .LazyLoad().AsBag();
        }
    }
}
