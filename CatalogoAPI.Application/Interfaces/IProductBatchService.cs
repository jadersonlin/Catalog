using System.Threading.Tasks;
using Catalog.Application.Dtos;

namespace Catalog.Application.Interfaces
{
    public interface IProductBatchService
    {
        Task<ProcessFileResult> ProcessFile(ProcessFileInput input);
    }
}
