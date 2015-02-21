using System;
using System.Collections.Generic;
using System.Globalization;
using JLT.JobScheduler.Host.MessageContexts;
using MassTransit;
using MassTransit.Context;
using Newtonsoft.Json;
using Quartz;

namespace JLT.JobScheduler.Host.Jobs
{
    /// <summary>
    /// THIS WAS WRITTEN IN AN ATTEMPT TO BE ABLE TO PUBLISH SCHEDULED EVENTS IN THE SAME WAY THAT MASSTRANSIT CAN SEND SCHEDULED MESSAGES (COMMANDS).
    /// ITS BASED ON MASSTRANSIT CODE (ScheduledMessageJob) BUT IS NOT COMPLETE AND DOES NOT WORK...
    /// </summary>
    public class ScheduledEventJob : IJob
    {
        readonly IServiceBus _bus;

        public ScheduledEventJob(IServiceBus bus)
        {
            _bus = bus;
        }

        public string Destination { get; set; }

        public string ExpirationTime { get; set; }
        public string ResponseAddress { get; set; }
        public string FaultAddress { get; set; }
        public string Body { get; set; }

        public string MessageId { get; set; }
        public string MessageType { get; set; }
        public string ContentType { get; set; }
        public string RequestId { get; set; }
        public string ConversationId { get; set; }
        public string CorrelationId { get; set; }
        public string Network { get; set; }
        public int RetryCount { get; set; }
        public string HeadersAsJson { get; set; }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var destinationAddress = new Uri(Destination);
                Uri sourceAddress = _bus.Endpoint.Address.Uri;

                var messageContext = CreateMessageContext(sourceAddress, destinationAddress);

                _bus.Publish(messageContext);
            }
            catch (Exception ex)
            {
                string message = string.Format(CultureInfo.InvariantCulture,
                    "An exception occurred sending message {0} to {1}", MessageType, Destination);

                throw new JobExecutionException(message, ex);
            }
        }

        IPublishContext CreateMessageContext(Uri sourceAddress, Uri destinationAddress)
        {
            var context = new ScheduledEventContext(Body);

            context.SetDestinationAddress(destinationAddress);
            context.SetSourceAddress(sourceAddress);
            context.SetResponseAddress(ToUri(ResponseAddress));
            context.SetFaultAddress(ToUri(FaultAddress));

            SetHeaders(context);
            context.SetMessageId(MessageId);
            context.SetRequestId(RequestId);
            context.SetConversationId(ConversationId);
            context.SetCorrelationId(CorrelationId);

            if (!string.IsNullOrEmpty(ExpirationTime))
                context.SetExpirationTime(DateTime.Parse(ExpirationTime));

            context.SetNetwork(Network);
            context.SetRetryCount(RetryCount);
            context.SetContentType(ContentType);

            return context;
        }

        void SetHeaders(ScheduledEventContext context)
        {
            if (string.IsNullOrEmpty(HeadersAsJson))
                return;

            var headers = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(HeadersAsJson);
            context.SetHeaders(headers);
        }

        static Uri ToUri(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return new Uri(s);
        }
    }
}
