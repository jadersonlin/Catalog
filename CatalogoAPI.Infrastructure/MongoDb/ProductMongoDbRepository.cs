using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories;
using MongoDB.Driver;

namespace Catalog.Infrastructure.MongoDb
{
    public class ProductMongoDbRepository : IProductRepository
    {
        private readonly ICatalogContext context;

        public ProductMongoDbRepository(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<Product> GetById(object id)
        {
            var lm = Convert.ToInt32(id);

            return await context
                .Products
                .Find(p => p.Lm == lm)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await context
                .Products
                .Find(p => true)
                .ToListAsync();
        }

        public async Task InsertMany(IList<Product> obj)
        {
            var listWrites = obj
                .Select(product => new InsertOneModel<Product>(product))
                .Cast<WriteModel<Product>>()
                .ToList();

            await context.Products.BulkWriteAsync(listWrites, new BulkWriteOptions
            {
                IsOrdered = false
            });
        }

        public async Task<bool> Update(Product obj)
        {
            var updateResult = await context
                .Products
                .ReplaceOneAsync(filter: g => g.Lm == obj.Lm, replacement: obj);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Remove(object id)
        {
            var lm = Convert.ToInt32(id);

            var filter = Builders<Product>.Filter.Eq(p => p.Lm, lm);
            
            var deleteResult = await context
                .Products
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
