//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.Http;
//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;
//using Microsoft.EntityFrameworkCore;
//using azurefunctions.Data;

//namespace azurefunctions.Functions
//{
//    public class ProductsGetAllCreate
//    {
//        private readonly AppDbContext _ctx;
//        public ProductsGetAllCreate(AppDbContext ctx)
//        {
//            _ctx = ctx;
//        }

//        [FunctionName("ProductsGetAllCreate")]
//        public async Task<IActionResult> Run(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "product")] HttpRequest req)
//        {

//            // Create
//            if (req.Method == HttpMethods.Post)
//            {
//                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//                var product = JsonConvert.DeserializeObject<Product>(requestBody);
//                _ctx.Products.Add(product);
//                await _ctx.SaveChangesAsync();
//                return new CreatedResult("/products", product);
//            }

//            // GetAll
//            var products = await _ctx.Products.ToListAsync();
//            return new OkObjectResult(products);
//        }
//    }
//}
