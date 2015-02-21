using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLT.BenefitCosts.Messages
{
    public class CreatedPolicyCost
    {
        public Guid Id { get; set; }

        public virtual decimal Cost { get; set; }

        public virtual decimal EmployerAmount { get; set; }

        public virtual decimal EmployeeAmount { get; set; }

        public virtual decimal TaxableAmount { get; set; }

        public virtual decimal Value { get; set; }

        public virtual decimal RequestedValue { get; set; }

        public virtual Guid? EligibilityGroupId { get; set; }

        public virtual Guid EmployeeBenefitPolicyId { get; set; }
    }
}
