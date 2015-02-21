using System;
using JLT.Framework.Service.Data;

namespace JLT.BenefitCosts.Domain.Companies
{
    public class BenefitSchemeCoverLevel : Entity
    {
        protected BenefitSchemeCoverLevel()
        {
            
        }

        public BenefitSchemeCoverLevel(Guid id)
        {
            base.Id = id;
        }

        public virtual Guid BenefitSchemeId { get; set; }

        public virtual string CostFormula { get; set; }
    }
}
