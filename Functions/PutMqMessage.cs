using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Windows.Storage.Streams;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace azurefunctions.Functions
{
    public static class PutMqMessage
    {
        /// <summary>
        /// Send the message posted on the request to IBM MQ.
        /// </summary>
        /// <param name="req">The HttpRequest.</param>
        /// <returns>The hex-encoded Id of the MQ message sent if successful, 500 otherwise.</returns>
        [FunctionName("PutMqMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "mqmessage")] HttpRequest req)
        {
            try
            {
                // mq send
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string idString = MQClient.SendMessage("DEV.QUEUE.1", requestBody);
                return new OkObjectResult($"Message id={idString} sent successfully.");
            }
            catch (Exception)
            {
                return new StatusCodeResult(500); // Internal Server Error
            }
        }
    }
}

