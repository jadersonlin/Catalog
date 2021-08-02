using System;
using System.Net.Http;
using System.Threading.Tasks;
using Catalog.API.Controllers;
using Catalog.Application.Dtos;
using Catalog.Application.Impl;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Catalog.Tests.Api
{
    public class ProductsControllerTests
    {
        private readonly Mock<ICatalogService> catalogServiceMock;

        public ProductsControllerTests()
        {
            catalogServiceMock = new Mock<ICatalogService>();
        }

        [Fact]
        public async Task GET_can_get_product_by_id()
        {
            //Arrange
            const int lm = 1;

            var product = new GetProductResult
            {
                Lm = lm,
                Name = "Product 1",
                Description = "Test",
                Price = 10,
                FreeShipping = true,
                CategoryId = 123
            };

            catalogServiceMock.Setup(_ => _.GetProduct(lm)).ReturnsAsync(product);

            //Act
            var controller = new ProductsController(catalogServiceMock.Object);
            var result = await controller.Get(lm);
            var actualResultObj = ((OkObjectResult)result.Result).Value;
            var actualResult = actualResultObj as GetProductResult;

            //Assert
            Assert.IsAssignableFrom<GetProductResult>(actualResult);
            Assert.IsType<int>(actualResult.Lm);
            Assert.IsType<string>(actualResult.Name);
            Assert.IsType<string>(actualResult.Description);
            Assert.IsType<decimal>(actualResult.Price);
            Assert.IsType<bool>(actualResult.FreeShipping);
            Assert.IsType<int>(actualResult.CategoryId);
        }

        [Fact]
        public async Task PUT_can_change_product()
        {
            //Arrange
            const int lm = 1;

            var product = new PutProductInput
            {
                Name = "Product 1",
                Description = "Test",
                Price = 10,
                FreeShipping = true,
                CategoryId = 123
            };

            catalogServiceMock.Setup(_ => _.EditProduct(product, lm)).ReturnsAsync(true);

            //Act
            var controller = new ProductsController(catalogServiceMock.Object);
            var result = await controller.Put(product, lm);
            var ok = result as OkObjectResult;

            //Assert
            Assert.NotNull(ok);
        }

        [Fact]
        public async Task DELETE_can_delete_product()
        {
            //Arrange
            const int lm = 1;

            catalogServiceMock.Setup(_ => _.DeleteProduct(lm)).ReturnsAsync(true);

            var controller = new ProductsController(catalogServiceMock.Object);
            var result = await controller.Delete(lm);
            var ok = result as OkObjectResult;

            //Assert
            Assert.NotNull(ok);
        }
    }
}
