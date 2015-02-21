using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MassTransit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;

namespace JLT.JobScheduler.Host.Extensions
{
    public static class SchedulerExtensions
    {
        public static void ScheduleJob<T>(this IScheduler scheduler, IConsumeContext context, Guid correlationId, Uri destination, string cronExpression)
            where T : IJob
        {
            var body = TranslateBody(context, destination);

            var jobDetail = JobBuilder.Create<T>()
                .RequestRecovery(true)
                .WithIdentity(correlationId.ToString("N"))
                .UsingJobData("Destination", ToString(destination))
                .UsingJobData("ResponseAddress", ToString(context.ResponseAddress))
                .UsingJobData("FaultAddress", ToString(context.FaultAddress))
                .UsingJobData("Body", body)
                .UsingJobData("ContentType", context.ContentType)
                .UsingJobData("MessageId", context.MessageId)
                .UsingJobData("RequestId", context.RequestId)
                .UsingJobData("ConversationId", context.ConversationId)
                .UsingJobData("CorrelationId", context.CorrelationId)
                .UsingJobData("HeadersAsJson", JsonConvert.SerializeObject(context.Headers))
                .UsingJobData("ExpirationTime",
                    context.ExpirationTime.HasValue ? context.ExpirationTime.Value.ToString() : "")
                .UsingJobData("Network", context.Network)
                .UsingJobData("RetryCount", context.RetryCount)
                .Build();

            var trigger = TriggerBuilder.Create()
                .ForJob(jobDetail)
                .WithCronSchedule(cronExpression)
                .WithIdentity(new TriggerKey(correlationId.ToString("N")))
                .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        private static string ToString(Uri uri)
        {
            if (uri == null)
                return "";

            return uri.ToString();
        }

        private static string TranslateBody(IConsumeContext context, Uri destination)
        {
            string body;
            using (var ms = new MemoryStream())
            {
                context.BaseContext.CopyBodyTo(ms);
                body = Encoding.UTF8.GetString(ms.ToArray());
            }

            if (string.Compare(context.ContentType, "application/vnd.masstransit+json", StringComparison.OrdinalIgnoreCase) == 0)
                body = TranslateJsonBody(body, destination.ToString());
            else if (string.Compare(context.ContentType, "application/vnd.masstransit+xml",
                StringComparison.OrdinalIgnoreCase) == 0)
                body = TranslateXmlBody(body, destination.ToString());
            else
                throw new InvalidOperationException("Only JSON and XML messages can be scheduled");
            return body;
        }

        private static string TranslateJsonBody(string body, string destination)
        {
            JObject envelope = JObject.Parse(body);

            envelope["destinationAddress"] = destination;

            JToken message = envelope["message"];

            JToken payload = message["payload"];
            JToken payloadType = message["payloadType"];

            envelope["message"] = payload;
            envelope["messageType"] = payloadType;

            return JsonConvert.SerializeObject(envelope, Formatting.Indented);
        }

        private static string TranslateXmlBody(string body, string destination)
        {
            using (var reader = new StringReader(body))
            {
                XDocument document = XDocument.Load(reader);

                XElement envelope = (from e in document.Descendants("envelope") select e).Single();

                XElement destinationAddress = (from a in envelope.Descendants("destinationAddress") select a).Single();

                XElement message = (from m in envelope.Descendants("message") select m).Single();
                IEnumerable<XElement> messageType = (from mt in envelope.Descendants("messageType") select mt);

                XElement payload = (from p in message.Descendants("payload") select p).Single();
                IEnumerable<XElement> payloadType = (from pt in message.Descendants("payloadType") select pt);

                message.Remove();
                messageType.Remove();

                destinationAddress.Value = destination;

                message = new XElement("message");
                message.Add(payload.Descendants());
                envelope.Add(message);

                envelope.Add(payloadType.Select(x => new XElement("messageType", x.Value)));

                return document.ToString();
            }
        }
    }
}
