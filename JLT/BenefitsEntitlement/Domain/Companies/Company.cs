using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JLT.Framework.Service.Data;

namespace JLT.BenefitCosts.Domain.Companies
{
    public class Company : Entity
    {
        protected Company()
        {
            
        }

        public Company(Guid id)
        {
            base.Id = id;
        }

        public virtual Collection<BenefitSchemeCoverLevel> BenefitSchemeCoverLevels { get; set; }
    }
}
