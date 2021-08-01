using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories;
using System.Threading.Tasks;

namespace Catalog.Application.Impl
{
    public class CatalogService : ICatalogService
    {
        private readonly IProductRepository repository;

        public CatalogService(IProductRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GetProductResult> GetProduct(int id)
        {
            var product = await repository.GetById(id);

            return MapDto(product);
        }

        private GetProductResult MapDto(Product product)
        {
            if (product == null)
                return null;

            return new GetProductResult
            {
                Lm = product.Lm,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                FreeShipping = product.FreeShipping
            };
        }

        public async Task<bool> EditProduct(PutProductInput input, int lm)
        {
            var product = MapProduct(input, lm);

            return await repository.Update(product);
        }

        private Product MapProduct(PutProductInput input, int lm)
        {
            return new Product(lm, input.Name, input.FreeShipping, input.Description, input.Price, input.CategoryId);
        }

        public async Task<bool> DeleteProduct(int lm)
        {
            return await repository.Remove(lm);
        }
    }
}
