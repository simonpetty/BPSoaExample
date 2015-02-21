using JLT.PensionsEntitlement.Messages.Events;
using MassTransit;
using Topshelf.Logging;

namespace JLT.PensionsEntitlement.Host.Consumers
{
    public class OnEmploymentStarted : Consumes<EmployeeStartedEmployment>.All
    {
        private static readonly LogWriter LogWriter = HostLogger.Get<OnEmploymentStarted>();

        public void Consume(EmployeeStartedEmployment message)
        {
            LogWriter.Info("! Employee Started Employment - " +message.EmployeeId + " !");
        }
    }
}
