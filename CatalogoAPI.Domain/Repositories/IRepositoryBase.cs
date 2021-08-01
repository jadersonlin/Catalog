using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.Repositories
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> GetById(object id);
        Task<IEnumerable<TEntity>> GetAll();
        Task InsertMany(IList<TEntity> obj);
        Task<bool> Update(TEntity obj);
        Task<bool> Remove(object id);
    }
}
