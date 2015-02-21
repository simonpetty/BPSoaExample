using System;
using System.Collections.ObjectModel;
using System.Linq;
using FluentNHibernate.Data;
using JLT.BenefitCosts.Domain.Events;
using JLT.Framework.Core.Events;

namespace JLT.BenefitCosts.Domain.Employees
{
    public class Employee : Entity
    {
        public virtual Guid CompanyId { get; set; }

        public virtual Collection<EmployeeBenefitCoverLevel> EmployeeBenefitCoverLevels { get; set; }

        [EventPublisher("EmployeeBenefitCostUpdated")]
        public static event EventHandler<EmployeeBenefitCostUpdatedEventArgs> EmployeeBenefitCostUpdated;

        public void SetBenefitCoverLevelCost(Guid schemeCoverLevelId, decimal cost)
        {
            var coverLevel = GetOrCreateBenefitCoverLevel(schemeCoverLevelId);
            
            if (coverLevel.Cost == cost)
                return;

            var previousCost = coverLevel.Cost;
            coverLevel.Cost = cost;

            EmployeeBenefitCostUpdated.Invoke(this, new EmployeeBenefitCostUpdatedEventArgs
            {
                BenefitSchemeId = coverLevel.SchemeCoverLevel.BenefitSchemeId,
                BenefitSchemeCoverLevelId = coverLevel.SchemeCoverLevel.Id,
                PreviousCost = previousCost,
                NewCost = coverLevel.Cost.Value
            });
        }

        public void SetBenefitCoverLevelValue(Guid schemeCoverLevelId, decimal value)
        {
            GetOrCreateBenefitCoverLevel(schemeCoverLevelId).Value = value;
        }

        private EmployeeBenefitCoverLevel GetOrCreateBenefitCoverLevel(Guid schemeCoverLevelId)
        {
            var cl = EmployeeBenefitCoverLevels.SingleOrDefault(x => x.SchemeCoverLevel.Id == schemeCoverLevelId);
            if (cl != null) 
                return cl;

            cl = new EmployeeBenefitCoverLevel();
            EmployeeBenefitCoverLevels.Add(cl);
            return cl;
        }
    }
}
