using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories;

namespace Catalog.Infrastructure.MongoDb
{
    public class FileMongoDbRepository : IFileRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<File> GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<File>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task InsertMany(IList<File> obj)
        {
            throw new NotImplementedException();
        }

        public Task InsertMany(IEnumerable<File> obj)
        {
            throw new NotImplementedException();
        }

        public Task Insert(File obj)
        {
            throw new NotImplementedException();
        }

        public Task Update(File obj)
        {
            throw new NotImplementedException();
        }

        public Task Remove(File obj)
        {
            throw new NotImplementedException();
        }
    }
}
