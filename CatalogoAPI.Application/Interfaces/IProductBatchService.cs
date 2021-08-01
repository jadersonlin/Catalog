using System.Threading.Tasks;

namespace Catalog.Application.Interfaces
{
    public interface IProductBatchService
    {
        Task ProcessFile();
    }
}
