using System;
using System.Threading;
using MassTransit;

namespace JLT.Framework.ServiceBus.Extensions
{
    public static class ConsumeContext
    {
        [ThreadStatic]
        private static IConsumeContext _consumeContext;

        public static IConsumeContext Current
        {
            get { return _consumeContext; }
            set { _consumeContext = value; }
        }
    }

    public static class ConsumeContextExtensions
    {
        public static string GetContextIdentity(this IConsumeContext consumeContext)
        {
            return consumeContext.Headers["JLT.ContextIdentity"];
        }
    }

    public static class SendContextExtensions
    {
        public static void SetContextIdentity(this ISendContext sendContext)
        {
            sendContext.SetHeader("JLT.ContextIdentity", Thread.CurrentPrincipal.Identity.Name);
        }
    }
}
