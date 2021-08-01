using System.Threading.Tasks;
using Catalog.Domain.Enums;

namespace Catalog.Application.Interfaces
{
    public interface IProductBatchService
    {
        Task<FileProcessingStatus?> ProcessFile();
    }
}
