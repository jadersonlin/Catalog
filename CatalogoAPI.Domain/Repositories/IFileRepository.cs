using System.Threading.Tasks;
using Catalog.Domain.Models;

namespace Catalog.Domain.Repositories
{
    public interface IFileRepository : IRepositoryBase<File>
    {
        Task Insert(File obj);
    }
}
