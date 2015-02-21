using System;
using JLT.BenefitCosts.Domain.Events;
using JLT.Framework.Core.Events;
using JLT.Framework.Service.Data;

namespace JLT.BenefitCosts.Domain.Employees
{
    public class EmployeeBenefitCoverLevel : Entity
    {
        public virtual Guid BenefitSchemeId { get; set; }

        public virtual Guid CoverLevelId { get; set; }

        private decimal? _latestCost;

        public virtual decimal? LatestCost
        {
            get { return _latestCost; }
        }

        [EventPublisher("EmployeeBenefitCostUpdated")]
        public static event EventHandler<EmployeeBenefitCostUpdatedEventArgs> EmployeeBenefitCostUpdated;

        public virtual void UpdateLatestCost(decimal cost)
        {
            var previousCost = LatestCost;
            _latestCost = cost;

            if (_latestCost != previousCost)
            {
                EmployeeBenefitCostUpdated.Invoke(this, new EmployeeBenefitCostUpdatedEventArgs
                    {
                        BenefitSchemeId = BenefitSchemeId,
                        BenefitSchemeCoverLevelId = CoverLevelId,
                        PreviousCost = previousCost,
                        NewCost = cost
                    });
            }
        }
    }
}
