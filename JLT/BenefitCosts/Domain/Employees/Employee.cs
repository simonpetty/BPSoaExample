using System;
using System.Collections.ObjectModel;
using System.Linq;
using JLT.Framework.Service.Data;

namespace JLT.BenefitCosts.Domain.Employees
{
    public class Employee : Entity
    {
        protected Employee()
        {
            
        }

        public Employee(Guid id)
        {
            base.Id = id;
        }

        public virtual Guid CompanyId { get; set; }

        public virtual Collection<EmployeeBenefitCoverLevel> EmployeeBenefitCoverLevels { get; set; }

        private EmployeeBenefitCoverLevel GetOrCreateEmployeeBenefitCoverLevel(Guid benefitSchemeId, Guid coverLevelId)
        {
            var cl = EmployeeBenefitCoverLevels.SingleOrDefault(x =>
                x.BenefitSchemeId == benefitSchemeId &&
                x.CoverLevelId == coverLevelId);

            if (cl != null) 
                return cl;

            cl = new EmployeeBenefitCoverLevel();
            EmployeeBenefitCoverLevels.Add(cl);
            return cl;
        }

        public virtual void SetBenefitCoverLevelCost(Guid benefitSchemeId, Guid coverLevelId, decimal cost)
        {
            var coverLevel = GetOrCreateEmployeeBenefitCoverLevel(benefitSchemeId, coverLevelId);
            coverLevel.UpdateLatestCost(cost);
        }
    }
}
