using JLT.Framework.ServiceBus;
using JLT.Framework.ServiceBus.Installers;

namespace JLT.BenefitCosts.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            new ServiceHostFactory<ServiceHost>()
                .With(new ServiceBusInstaller())
                .With(new SessionFactoryInstaller())
                .Run();
        }
    }
}
