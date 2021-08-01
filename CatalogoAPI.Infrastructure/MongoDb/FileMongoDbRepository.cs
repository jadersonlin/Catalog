using Catalog.Domain.Models;
using Catalog.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Catalog.Infrastructure.MongoDb
{
    public class FileMongoDbRepository : IFileRepository
    {
        private readonly ICatalogContext context;

        public FileMongoDbRepository(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<File> GetById(object id)
        {
            return await context
                .Files
                .Find(p => p.Id == id.ToString())
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<File>> GetAll()
        {
            return await context
                .Files
                .Find(p => true)
                .ToListAsync();
        }

        public async Task InsertMany(IList<File> obj)
        {
            await context.Files.InsertManyAsync(obj);
        }


        public async Task Insert(File obj)
        {
            await context.Files.InsertOneAsync(obj);
        }

        public async Task<bool> Update(File obj)
        {
            var updateResult = await context
                .Files
                .ReplaceOneAsync(filter: g => g.Id == obj.Id, replacement: obj);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Remove(object id)
        {
            var filter = Builders<File>.Filter.Eq(p => p.Id, id.ToString());

            var deleteResult = await context
                .Files
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
