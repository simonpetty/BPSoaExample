using System.Collections.Generic;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor.Installer;
using Topshelf;

namespace JLT.Framework.ServiceBus
{
    public class ServiceHostFactory<T> where T : ServiceHost, new()
    {
        private readonly List<IWindsorInstaller> _installers = new List<IWindsorInstaller>();
        private readonly Assembly _callingAssembly;

        public ServiceHostFactory()
        {
            _callingAssembly = Assembly.GetCallingAssembly();
            _installers.Add(FromAssembly.Instance(_callingAssembly));
        }

        public ServiceHostFactory<T> With(IWindsorInstaller installer)
        {
            _installers.Add(installer);
            return this;
        }

        public void Run()
        {
            HostFactory.Run(x =>
            {
                x.Service<T>(s =>
                {
                    s.ConstructUsing(name => new T());
                    s.WhenStarted(tc => tc.Start(_installers.ToArray()));
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();

                var callingAssemblyName = _callingAssembly.GetName();

                x.SetDescription("MassTransit RabbitMQ Service Bus Host");
                x.SetDisplayName(callingAssemblyName.Name);
                x.SetServiceName(callingAssemblyName.Name);

                x.EnableServiceRecovery(rc =>
                {
                    rc.RestartService(1); // restart the service after 1 minute
                    //rc.RestartComputer(1, "System is restarting!"); // restart the machine after 1 minute
                    //rc.RunProgram(1, "notepad.exe"); // run a program
                    //rc.SetResetPeriod(1); // set the reset interval to one day
                });
            });
        }
    }
}
