using Castle.MicroKernel.Registration;
using Castle.Windsor;
using JLT.Framework.Core.Logging;
using JLT.Framework.Core.Logging.Log4Net;
using JLT.Framework.Core.ServiceAgent;
using MassTransit;

namespace JLT.Framework.ServiceBus
{
    public class ServiceHost
    {
        protected IWindsorContainer Container;
        private IServiceBus _bus;

        public virtual void Start(IWindsorInstaller[] installers)
        {
            Container = new WindsorContainer();

            ServiceLocator.InitializeContainer(Container);

            Container.Install(installers);

            Container.Register(Component.For<ILoggerFactory>()
                .UsingFactoryMethod(() => new Log4NetLoggerFactory()).LifestyleSingleton());

            // Ensure the service bus is running on startup
            if (Container.Kernel.HasComponent(typeof (IServiceBus)))
                _bus = Container.Resolve<IServiceBus>();
        }

        public virtual void Stop()
        {
            if (_bus != null)
                _bus.Dispose();
        }
    }
}
