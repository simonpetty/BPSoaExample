using System;
using JLT.BenefitCosts.Host.Messages;
using MassTransit;

namespace JLT.BenefitCosts.Host.Consumers
{
    public class EvaluateBenefitRateFormula : Consumes<EvaluateBenefitValueRequest>.All
    {
        public IServiceBus Bus { get; set; }

        public void Consume(EvaluateBenefitValueRequest message)
        {
            Bus.Endpoint.Send(new EvaluateBenefitValueResponse
                {
                    Value = DateTime.Now.Second * DateTime.Now.Ticks * DateTime.Now.Millisecond
                });
        }
    }
}
