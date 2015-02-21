using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topshelf;

namespace JLT.BenefitCosts.Host
{
    /// <summary>
    /// Entry point of the host process used to construct the Topshelf service.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ServiceHost>(s =>
                {
                    s.ConstructUsing(name => new ServiceHost());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("MassTransit RabbitMQ Service Bus Host");
                x.SetDisplayName("BenefitCosts Host");
                x.SetServiceName("BenefitCostsHost");
            });


            //TODO: extra parameter stuff 

        }
    }
}
