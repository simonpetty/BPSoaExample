using JLT.Framework.ServiceBus;
using JLT.Framework.ServiceBus.Installers;

namespace JLT.JobScheduler.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            new ServiceHostFactory<SchedulerServiceHost>()
                .With(new ServiceBusInstaller())
                .Run();
        }
    }
}
