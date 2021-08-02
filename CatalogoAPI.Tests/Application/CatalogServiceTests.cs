using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Impl;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories;
using Moq;
using Xunit;

namespace Catalog.Tests.Application
{
    public class CatalogServiceTests
    {

        private readonly Mock<IProductRepository> productRepositoryMock;

        public CatalogServiceTests()
        {
            productRepositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task Can_get_product()
        {
            const int id = 1;

            var product = new Product(id, "Test product", true, "Test", 10, 1);

            productRepositoryMock.Setup(_ => _.GetById(id)).ReturnsAsync(product);

            var service = new CatalogService(productRepositoryMock.Object);
            var result = await service.GetProduct(id);

            Assert.NotNull(result);
            Assert.Equal(product.Lm, result.Lm);
            Assert.Equal(product.CategoryId, result.CategoryId);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.FreeShipping, result.FreeShipping);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
        }

        [Fact]
        public async Task Try_get_null_product()
        {
            const int id = 1;

            productRepositoryMock.Setup(_ => _.GetById(id)).ReturnsAsync((Product)null);

            var service = new CatalogService(productRepositoryMock.Object);
            var result = await service.GetProduct(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task Can_edit_product()
        {
            const int id = 1;

            var productInput = new PutProductInput
            {
                Name = "Test product",
                Description = "Test",
                Price = 10,
                FreeShipping = true,
                CategoryId = 1
            };

            var productModel = new Product(id, "Test product", true, "Test", 10, 1);

            productRepositoryMock.Setup(_ => _.Update(productModel)).ReturnsAsync(true);

            var service = new CatalogService(productRepositoryMock.Object);
            var result = await service.EditProduct(productInput, id);

            Assert.True(result);
        }

        [Fact]
        public async Task Error_editing_product()
        {
            const int id = 1;

            var productInput = new PutProductInput
            {
                Name = "Test product",
                Description = "Test",
                Price = 10,
                FreeShipping = true,
                CategoryId = 1
            };

            var productModel = new Product(id, "Test product", true, "Test", 10, 1);

            productRepositoryMock.Setup(_ => _.Update(productModel)).ReturnsAsync(false);

            var service = new CatalogService(productRepositoryMock.Object);
            var result = await service.EditProduct(productInput, id);

            Assert.False(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Can_delete_product(bool expectedResult)
        {
            const int id = 1;

            productRepositoryMock.Setup(_ => _.Remove(id)).ReturnsAsync(expectedResult);

            var service = new CatalogService(productRepositoryMock.Object);
            var result = await service.DeleteProduct(id);

            Assert.Equal(expectedResult, result);
        }
    }
}
