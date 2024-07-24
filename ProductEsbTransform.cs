using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Google.Protobuf.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace azurefunctions
{
    public class ProductEsbTransform
    {
        [FunctionName("ProductEsbTransform")]
        public string Run([ServiceBusTrigger("myqueue", Connection = "SvcBusCnxString")] ServiceBusReceivedMessage message)
        {
            var id = message.MessageId;
            var body = message.Body;
            var contentType = message.ContentType;

            // do translation here
            var outputMessage = $"Message ID: {id}, Message Body: {body}, Message Content-Type: {contentType}";

            return outputMessage;
        }
    }
}
