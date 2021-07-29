using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories;

namespace Catalog.Infrastructure.MongoDb
{
    public class ProductMongoDbRepository : IProductRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(Product obj)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Product obj)
        {
            throw new NotImplementedException();
        }
    }
}
