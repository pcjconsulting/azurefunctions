using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace azurefunctions.Functions
{
    /// <summary>
    /// Get the MQ message for the specified ID.
    /// </summary>
    /// <param name="req">The HttpRequest.</param>
    /// <param name="id">The hex-encoded Id of the MQ message.</param>
    /// <returns>The MQ message if successful, 500 otherwise</returns>
    public static class GetMqMessage
    {
        [FunctionName("GetMqMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mqmessage/{id}")] HttpRequest req, string id)
        {
            try
            {
                // mq receive
                string receivedMessage = MQClient.GetMessage("DEV.QUEUE.1", id);
                return new OkObjectResult(receivedMessage);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500); // Internal Server Error
            }
        }
    }
}
