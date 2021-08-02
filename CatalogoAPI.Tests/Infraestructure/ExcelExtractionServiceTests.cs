using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Infrastructure.Extraction;
using Moq;
using Xunit;

namespace Catalog.Tests.Infraestructure
{
    public class ExcelExtractionServiceTests
    {
        private readonly Mock<IStorageService> storageServiceMock;

        public ExcelExtractionServiceTests()
        {
            storageServiceMock = new Mock<IStorageService>();
        }

        [Fact]
        public async Task Can_extract_valid_file()
        {
            //Arrange
            var fileId = Guid.NewGuid().ToString();
            const string fileName = "products_test_no_errors.xlsx";
            var stream = GetTestFile(fileName);
            storageServiceMock.Setup(_ => _.GetFile(fileId)).ReturnsAsync(stream);

            //Act
            IExtractionService excelExtractionService = new ExcelExtractionService(storageServiceMock.Object);
            var (products, validationErrors) = await excelExtractionService.ExtractDataFromFile(fileId);

            //Assert
            Assert.IsAssignableFrom<List<ProductData>>(products);
            Assert.IsAssignableFrom<List<KeyValuePair<string, string>>>(validationErrors);
            Assert.NotEmpty(products);
            Assert.Empty(validationErrors);
        }

        [Fact]
        public async Task Try_extract_invalid_file()
        {
            //Arrange
            var fileId = Guid.NewGuid().ToString();
            const string fileName = "products_test_with_errors.xlsx";
            var stream = GetTestFile(fileName);
            storageServiceMock.Setup(_ => _.GetFile(fileId)).ReturnsAsync(stream);

            //Act
            IExtractionService excelExtractionService = new ExcelExtractionService(storageServiceMock.Object);
            var (products, validationErrors) = await excelExtractionService.ExtractDataFromFile(fileId);

            //Assert
            Assert.IsAssignableFrom<List<ProductData>>(products);
            Assert.IsAssignableFrom<List<KeyValuePair<string, string>>>(validationErrors);
            Assert.Empty(products);
            Assert.NotEmpty(validationErrors);
            Assert.Equal(5, validationErrors.Count);
        }

        private FileStream GetTestFile(string fileName)
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "TestFiles\\" + fileName;
            var stream = File.OpenRead(path);
            stream.Position = 0;
            return stream;
        }
    }
}
