using System;
using JLT.Framework.Service.Data;

namespace JLT.BenefitCosts.Domain.Employees
{
    public class PolicyCost : Entity
    {
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
