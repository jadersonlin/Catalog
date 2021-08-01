using Catalog.Domain.Models;
using MongoDB.Driver;

namespace Catalog.Infrastructure.MongoDb
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }

        IMongoCollection<File> Files { get; }
    }
}
