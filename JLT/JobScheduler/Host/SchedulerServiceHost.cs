using Castle.MicroKernel.Registration;
using JLT.Framework.ServiceBus;
using MassTransit;
using MassTransit.QuartzIntegration;
using Quartz;

namespace JLT.JobScheduler.Host
{
    public class SchedulerServiceHost : ServiceHost
    {
        private IScheduler _scheduler;
        private IServiceBus _bus;

        public override void Start(IWindsorInstaller[] installers)
        {
            base.Start(installers);

            _scheduler = Container.Resolve<IScheduler>();
            _bus = Container.Resolve<IServiceBus>();

            _scheduler.JobFactory = new MassTransitJobFactory(_bus);
            _scheduler.Start();
        }

        public override void Stop()
        {
            _scheduler.Standby();
            _bus.Dispose();
            _scheduler.Shutdown();
        }
    }
}
