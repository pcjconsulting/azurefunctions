using Azure.Data.Tables;
using Azure;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace azurefunctions.Data
{
    public class Helpers
    {
        public const string PartitionKey = "12";

        // Get all products
        public static async Task<List<Product>> GetProductsAsync(TableClient tableClient)
        {
            List<Product> products = new List<Product>();
            AsyncPageable<ProductEntity> queryResults = tableClient.QueryAsync<ProductEntity>(filter: $"PartitionKey eq '{PartitionKey}'");
            await foreach (ProductEntity item in queryResults)
            {
                var product = JsonConvert.DeserializeObject<Product>(item.Text);
                if (product != null)
                {
                    products.Add(product);
                }
            }
            return products;
        }


        // Scan to find product by ID.
        public static async Task<ProductEntity> GetEntityByIdAsync(TableClient tableClient, int id)
        {
            AsyncPageable<ProductEntity> queryResults = tableClient.QueryAsync<ProductEntity>(filter: $"PartitionKey eq '{PartitionKey}'");
            await foreach (ProductEntity item in queryResults)
            {
                var product = JsonConvert.DeserializeObject<Product>(item.Text);
                if (product?.ID == id)
                {
                    return item;
                }
            }
            return null;
        }

    }
}
