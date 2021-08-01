using Catalog.Domain.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.MongoDb
{
    public class CatalogMongoDbContext : ICatalogContext
    {
        public CatalogMongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("MongoDb:ConnectionString").Value);
            var database = client.GetDatabase(configuration.GetSection("MongoDb:Database").Value);

            Products = database.GetCollection<Product>("products");
            Files = database.GetCollection<File>("files");
        }

        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<File> Files { get; }
    }
}
