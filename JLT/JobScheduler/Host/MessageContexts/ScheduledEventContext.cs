using System;
using System.Collections.Generic;
using System.IO;
using MassTransit;
using MassTransit.Context;
using MassTransit.QuartzIntegration;
using MassTransit.Serialization.Custom;

namespace JLT.JobScheduler.Host.MessageContexts
{
    /// <summary>
    /// THIS IS PART OF AN ATTEMPT TO BE ABLE TO PUBLISH SCHEDULED EVENTS IN THE SAME WAY AS MASSTRANSIT SENDS SCHEDULED MESSAGES (COMMANDS)
    /// IT'S INCOMPLETE, MAINLY AROUND HOW TO HANDLE SUBSCRIPTIONS - SO DOES NOT WORK...
    /// </summary>
    public class ScheduledEventContext : MessageContext, IPublishContext
    {
        readonly string _body;
        readonly HashSet<Uri> _endpoints = new HashSet<Uri>();
        Action<IEndpointAddress> _eachSubscriberAction = Ignore;

        public ScheduledEventContext(string body)
        {
            _body = body;
            Id = NewId.NextGuid();
            DeclaringMessageType = typeof(ScheduledMessageContext);
        }

        static void Ignore(IEndpointAddress endpoint)
        {
        }

        public void ForEachSubscriber(Action<IEndpointAddress> action)
        {
            _eachSubscriberAction = action;
        }

        public void IfNoSubscribers(Action callback)
        {

        }

        public bool WasEndpointAlreadySent(IEndpointAddress address)
        {
            return _endpoints.Contains(address.Uri);
        }

        public Type DeclaringMessageType { get; set; }

        public DeliveryMode DeliveryMode { get; set; }

        public Guid Id { get; set; }

        public void NotifySend(IEndpointAddress address)
        {
            _endpoints.Add(address.Uri);

            _eachSubscriberAction(address);
        }

        public void SerializeTo(System.IO.Stream stream)
        {
            using (var nonClosingStream = new NonClosingStream(stream))
            using (var writer = new StreamWriter(nonClosingStream))
            {
                writer.Write(_body);
            }
        }

        public void SetDeliveryMode(DeliveryMode deliveryMode)
        {
            DeliveryMode = deliveryMode;
        }

        public bool TryGetContext<T>(out IBusPublishContext<T> context) where T : class
        {
            context = null;
            return false;
        }

        public void SetHeaders(IEnumerable<KeyValuePair<string, string>> headers)
        {
            foreach (var header in headers)
            {
                SetHeader(header.Key, header.Value);
            }
        }
    }
}
