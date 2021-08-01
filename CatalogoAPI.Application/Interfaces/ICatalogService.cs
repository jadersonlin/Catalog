using Catalog.Application.Dtos;
using System.Threading.Tasks;

namespace Catalog.Application.Interfaces
{
    public interface ICatalogService
    {
        Task<GetProductResult> GetProduct(int lm);
        Task<bool> EditProduct(PutProductInput input, int lm);
        Task<bool> DeleteProduct(int lm);
    }
}
