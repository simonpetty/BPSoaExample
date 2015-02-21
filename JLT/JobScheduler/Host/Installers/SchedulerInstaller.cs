using Castle.MicroKernel.Registration;
using Quartz;
using Quartz.Impl;

namespace JLT.JobScheduler.Host.Installers
{
    public class SchedulerInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<IScheduler>()
                .UsingFactoryMethod<IScheduler>(() => 
                    new StdSchedulerFactory().GetScheduler()));
        }
    }
}
