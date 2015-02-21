using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JLT.BenefitCosts.Domain.Events;
using JLT.Framework.Core.Events;

namespace JLT.BenefitCosts.Host.EventSubscribers
{
    public class EmployeeEventSubscriber
    {
        [EventSubscriber("EmployeeBenefitCostUpdated")]
        public void OnBenefitCoverLevelCostUpdated(object sender, EmployeeBenefitCostUpdatedEventArgs eventArgs)
        {
            
        }
    }
}
