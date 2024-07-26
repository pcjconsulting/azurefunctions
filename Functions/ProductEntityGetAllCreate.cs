using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Routing;
using Azure.Data.Tables;
using azurefunctions.Data;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace azurefunctions.Functions
{
    public static class ProductEntityGetAllCreate
    {
        [FunctionName("TableGetAllCreate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "productentity")] HttpRequest req,
            [Table("Product", Take = 5, Connection = "AzureWebJobsStorage")] TableClient tableClient)
        {
            try
            {
                // Create
                if (req.Method == HttpMethods.Post)
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    var entity = new ProductEntity
                    {
                        Text = requestBody,
                    };
                    var queryResults = await tableClient.AddEntityAsync<ProductEntity>(entity);
                    return new OkObjectResult(queryResults);
                }

                // GetAll
                else
                {
                    List<Product> products = await Helpers.GetProductsAsync(tableClient);
                    if (products == null || products.Count < 1)
                    {
                        return new OkObjectResult($"No Products found.");
                    }
                    return new OkObjectResult(products);
                }
            }
            catch (Exception)
            {
                return new StatusCodeResult(500); // Internal Server Error
            }
        }
    }
}
