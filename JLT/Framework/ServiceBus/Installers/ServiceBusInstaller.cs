using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using JLT.Framework.ServiceBus.Extensions;
using JLT.Framework.ServiceBus.Interceptors;
using MassTransit;
using MassTransit.Advanced;
using MassTransit.Saga;

namespace JLT.Framework.ServiceBus.Installers
{
    public class ServiceBusInstaller : IWindsorInstaller
    {
        private readonly Assembly _callingAssembly;

        public ServiceBusInstaller()
        {
            _callingAssembly = Assembly.GetCallingAssembly();
        }

        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes.FromAssembly(_callingAssembly).BasedOn<IConsumer>());
            container.Register(Classes.FromAssembly(_callingAssembly).BasedOn<ISaga>());

            // Will need to get the NHibernateSagaRepository working eventually
            container.Register(Component.For(typeof(ISagaRepository<>))
                .ImplementedBy(typeof(InMemorySagaRepository<>)));

            var concurrentConsumers = Environment.ProcessorCount*2;
            var prefetch = concurrentConsumers;

            container.Register(Component.For<IServiceBus>()
                .UsingFactoryMethod(() => ServiceBusFactory.New(sbc =>
                {
                    sbc.UseRabbitMq();
                    sbc.ReceiveFrom("rabbitmq://localhost/" + _callingAssembly.GetName().Name + "?prefetch=" + prefetch);
                    sbc.UseJsonSerializer();
                    sbc.SetConcurrentReceiverLimit(concurrentConsumers);
                    sbc.Subscribe(x => x.LoadFrom(container));
                    //sbc.UseLog4Net(); <- this needs log4net 1.2.13.0 (which has a different public key to the one framework uses)
                    sbc.AddInboundInterceptor(new ConsumeContextInboundInterceptor());
                    sbc.AddOutboundInterceptor(new HeaderForwardingOutboundInterceptor());
                }))
                .LifeStyle.Singleton);
        }
    }
}
