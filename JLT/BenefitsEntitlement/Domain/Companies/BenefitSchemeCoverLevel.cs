using System;
using JLT.Framework.Service.Data;

namespace JLT.BenefitCosts.Domain.Companies
{
    public class BenefitSchemeCoverLevel : Entity
    {
        public virtual Guid BenefitSchemeId { get; set; }

        public virtual string CostFormula { get; set; }

        public virtual string ValueFormula { get; set; }
    }
}
