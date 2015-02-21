using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JLT.Framework.Service.Data;

namespace JLT.BenefitCosts.Domain.Companies
{
    public class Company : Entity
    {
        private readonly ICollection<BenefitSchemeCoverLevel> _benefitSchemeCoverLevels = new Collection<BenefitSchemeCoverLevel>();

        protected Company()
        {
            
        }

        public Company(Guid id)
        {
            base.Id = id;
        }

        public virtual IEnumerable<BenefitSchemeCoverLevel> BenefitSchemeCoverLevels
        {
            get { return _benefitSchemeCoverLevels.Skip(0); }
        }

        public virtual void AddBenefitSchemeCoverLevel(BenefitSchemeCoverLevel benefitSchemeCoverLevel)
        {
            _benefitSchemeCoverLevels.Add(benefitSchemeCoverLevel);
        }
    }
}
