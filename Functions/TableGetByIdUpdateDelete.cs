using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using azurefunctions.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.IO;
using Azure.Data.Tables;
using Azure;

namespace azurefunctions.Functions
{
    public class TableGetByIdUpdateDelete
    {
        [FunctionName("TableGetByIdUpdateDelete")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", "delete", Route = "product/{id}")] HttpRequest req, int id,
            [Table("Product", Take=5, Connection = "AzureWebJobsStorage")] TableClient tableClient)
        {
            try
            {
                // Query table
                ProductEntity entity = await Helpers.GetEntityByIdAsync(tableClient, id);
                if (entity == null)
                {
                    return new OkObjectResult($"Product id={id} not found.");
                }
                // Get
                if (req.Method == HttpMethods.Get)
                {
                    return new OkObjectResult(entity.Text);
                }
                // Update
                else if (req.Method == HttpMethods.Put)
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    entity.Text = requestBody;
                    var response = await tableClient.UpdateEntityAsync<ProductEntity>(entity, entity.ETag);
                    return new OkObjectResult("Product entry updated successfully.");
                }
                // Delete
                else
                {
                    var response = await tableClient.DeleteEntityAsync(Helpers.PartitionKey, entity.RowKey);

                    return new OkObjectResult(response);
                }

            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500); // Internal Server Error
            }
        }

    }

}
