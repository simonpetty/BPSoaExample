using MassTransit;
using MassTransit.BusConfigurators;
using MassTransit.Pipeline.Configuration;

namespace JLT.Framework.ServiceBus.Extensions
{
    public static class ServiceBusConfiguratorExtensions
    {
        public static void AddOutboundInterceptor(this ServiceBusConfigurator configurator,
            IOutboundMessageInterceptor interceptor)
        {
            var builderConfigurator = new PostCreateBusBuilderConfigurator(bus =>
            {
                var interceptorConfigurator = new OutboundMessageInterceptorConfigurator(bus.OutboundPipeline);

                interceptorConfigurator.Create(interceptor);
            });

            configurator.AddBusConfigurator(builderConfigurator);
        }
    }
}
