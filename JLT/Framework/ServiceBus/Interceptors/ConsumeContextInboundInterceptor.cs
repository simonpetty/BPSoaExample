using JLT.Framework.ServiceBus.Extensions;
using MassTransit;

namespace JLT.Framework.ServiceBus.Interceptors
{
    class ConsumeContextInboundInterceptor : IInboundMessageInterceptor
    {
        public void PreDispatch(IConsumeContext context)
        {
            ConsumeContext.Current = context;
        }

        public void PostDispatch(IConsumeContext context)
        {
            ConsumeContext.Current = null;
        }
    }
}
