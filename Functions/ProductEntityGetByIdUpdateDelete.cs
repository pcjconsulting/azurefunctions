using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using azurefunctions.Data;
using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;

namespace azurefunctions.Functions
{
    public class ProductEntityGetByIdUpdateDelete
    {
        [FunctionName("ProductEntityGetByIdUpdateDelete")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", "delete", Route = "productentity/{id}")] HttpRequest req, int id,
            [Table("Product", Take = 5, Connection = "AzureWebJobsStorage")] TableClient tableClient)
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
                    await tableClient.UpdateEntityAsync<ProductEntity>(entity, entity.ETag);
                    return new OkObjectResult($"Product id={id} updated successfully.");
                }

                // Delete
                else
                {
                    await tableClient.DeleteEntityAsync(Helpers.PartitionKey, entity.RowKey);
                    return new OkObjectResult($"Product id={id} deleted successfully.");
                }
            }
            catch (Exception)
            {
                return new StatusCodeResult(500); // Internal Server Error
            }
        }
    }
}
