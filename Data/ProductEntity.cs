using Azure;
using System;
using Azure.Data.Tables;

namespace azurefunctions.Data
{
    public class ProductEntity : ITableEntity
    {
        public ProductEntity()
        {
            PartitionKey = Helpers.PartitionKey;
            RowKey = $"{Helpers.PartitionKey}-{Guid.NewGuid()}";
        }

        public string Text { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

    }
}
