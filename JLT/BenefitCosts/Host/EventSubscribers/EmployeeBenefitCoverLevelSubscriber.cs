using JLT.BenefitCosts.Domain.Events;
using JLT.Framework.Core.Events;
using MassTransit;

namespace JLT.BenefitCosts.Host.EventSubscribers
{
    public class EmployeeBenefitCoverLevelSubscriber
    {
        public IServiceBus Bus { get; set; }

        [EventSubscriber("EmployeeBenefitCostUpdated")]
        public void OnBenefitCoverLevelCostUpdated(object sender, EmployeeBenefitCostUpdatedEventArgs eventArgs)
        {
            Bus.Publish(eventArgs);
        }
    }
}
