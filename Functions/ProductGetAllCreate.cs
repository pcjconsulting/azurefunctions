using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using azurefunctions.Data;

namespace azurefunctions.Functions
{
    public class ProductGetAllCreate
    {
        private readonly AppDbContext _ctx;

        // Use paramless ctor till we get EF and SQL Server configured.
        //public ProductGetAllCreate(AppDbContext ctx)
        //{
        //    _ctx = ctx;
        //}
        public ProductGetAllCreate()
        {
            _ctx = null;
        }

        [FunctionName("ProductGetAllCreate")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "product")] HttpRequest req)
        {
            try
            {
                if (_ctx == null)
                {
                    return new OkObjectResult("The 'product' endpoint is not available, try the 'productentity' endpoint instead."); // Internal Server Error
                }

                // Create
                if (req.Method == HttpMethods.Post)
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    var product = JsonConvert.DeserializeObject<Product>(requestBody);
                    _ctx.Products.Add(product);
                    await _ctx.SaveChangesAsync();
                    return new CreatedResult("/product", product);
                }

                // GetAll
                var products = await _ctx.Products.ToListAsync();
                return new OkObjectResult(products);
            }
            catch (Exception)
            {
                return new OkObjectResult("The 'product' endpoint is not available, try the 'productentity' endpoint instead."); // Internal Server Error
            }
        }
    }
}
