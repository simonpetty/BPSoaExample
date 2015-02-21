using System.Linq;
using JLT.Framework.ServiceBus.Extensions;
using MassTransit;

namespace JLT.Framework.ServiceBus.Interceptors
{
    public class HeaderForwardingOutboundInterceptor : IOutboundMessageInterceptor
    {
        public void PreDispatch(ISendContext context)
        {
            var receiveContext = ConsumeContext.Current;

            if (receiveContext != null)
            {
                foreach (var header in receiveContext.Headers.Where(x => x.Key.StartsWith("JLT")))
                {
                    context.SetHeader(header.Key, header.Value);
                }
            }
            else
            {
                context.SetContextIdentity();
            }
        }

        public void PostDispatch(ISendContext context)
        {
        }
    }
}
