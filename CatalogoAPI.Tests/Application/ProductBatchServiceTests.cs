using Catalog.Application.Dtos;
using Catalog.Application.Impl;
using Catalog.Application.Interfaces;
using Catalog.Domain.Enums;
using Catalog.Domain.Repositories;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.Tests.Application
{
    public class ProductBatchServiceTests
    {
        private readonly Mock<IQueueService> queueServiceMock;
        private readonly Mock<IExtractionService> extractionServiceMock;
        private readonly Mock<IFileRepository> fileRepositoryMock;
        private readonly Mock<IProductRepository> productRepositoryMock;


        public ProductBatchServiceTests()
        {
            this.queueServiceMock = new Mock<IQueueService>();
            this.extractionServiceMock = new Mock<IExtractionService>();
            this.fileRepositoryMock = new Mock<IFileRepository>();
            this.productRepositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task Can_process_file()
        {
            //Arrange
            var uploadFileResult = new UploadFileResult
            {
                Id = Guid.NewGuid().ToString(),
                FileName = "file.xlsx",
                UploadedAt = DateTime.Now,
                Length = 100,
                Message = "teste"
            };
            var jsonFile = JsonConvert.SerializeObject(uploadFileResult);

            queueServiceMock.Setup(_ => _.DequeueMessage()).ReturnsAsync(new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                MessageText = jsonFile,
                PopReceipt = Guid.NewGuid().ToString()
            });

            var products = new List<ProductData>
            {
                new ProductData
                {
                    Lm = 1001,
                    Name = "Test Product 1",
                    CategoryId = 123,
                    FreeShipping = true,
                    Description = "teste description 1",
                    Price = (decimal)100.12
                }
            };

            var validationErrors = new List<KeyValuePair<string, string>>();

            var fileData = new Tuple<IList<ProductData>, IList<KeyValuePair<string, string>>>(products, validationErrors);

            extractionServiceMock.Setup(_ => _.ExtractDataFromFile(uploadFileResult.Id)).ReturnsAsync(fileData);

            //Act
            var productBatchService = new ProductBatchService(queueServiceMock.Object, extractionServiceMock.Object,
                fileRepositoryMock.Object, productRepositoryMock.Object);

            var result = await productBatchService.ProcessFile();

            Assert.IsType<FileProcessingStatus>(result);
            Assert.Equal(FileProcessingStatus.Processed, result);
        }

        [Fact]
        public async Task Try_process_file_with_validation_errors()
        {
            //Arrange
            var uploadFileResult = new UploadFileResult
            {
                Id = Guid.NewGuid().ToString(),
                FileName = "file.xlsx",
                UploadedAt = DateTime.Now,
                Length = 100,
                Message = "teste"
            };
            var jsonFile = JsonConvert.SerializeObject(uploadFileResult);

            queueServiceMock.Setup(_ => _.DequeueMessage()).ReturnsAsync(new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                MessageText = jsonFile,
                PopReceipt = Guid.NewGuid().ToString()
            });

            var products = new List<ProductData>();

            var validationErrors = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Error in line 1", "Value X is not valid.")
            };

            var fileData = new Tuple<IList<ProductData>, IList<KeyValuePair<string, string>>>(products, validationErrors);

            extractionServiceMock.Setup(_ => _.ExtractDataFromFile(uploadFileResult.Id)).ReturnsAsync(fileData);

            //Act
            var productBatchService = new ProductBatchService(queueServiceMock.Object, extractionServiceMock.Object,
                fileRepositoryMock.Object, productRepositoryMock.Object);

            var result = await productBatchService.ProcessFile();

            Assert.IsType<FileProcessingStatus>(result);
            Assert.Equal(FileProcessingStatus.InvalidFile, result);
        }

        [Fact]
        public async Task Try_process_with_no_message()
        {
            //Arrange
            queueServiceMock.Setup(_ => _.DequeueMessage()).ReturnsAsync((Message)null);

            //Act
            var productBatchService = new ProductBatchService(queueServiceMock.Object, extractionServiceMock.Object,
                fileRepositoryMock.Object, productRepositoryMock.Object);
            var result = await productBatchService.ProcessFile();

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Try_process_with_not_desserializable_message()
        {
            //Arrange
            queueServiceMock.Setup(_ => _.DequeueMessage()).ReturnsAsync(new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                MessageText = "{ invalid json object "
            });

            //Act and Assert
            var productBatchService = new ProductBatchService(queueServiceMock.Object, extractionServiceMock.Object,
                fileRepositoryMock.Object, productRepositoryMock.Object);

            await Assert.ThrowsAsync<InvalidDataException>(() => productBatchService.ProcessFile());
        }

        [Fact]
        public async Task Try_process_with_null_message_content()
        {
            //Arrange
            queueServiceMock.Setup(_ => _.DequeueMessage()).ReturnsAsync(new Message
            {
                MessageId = Guid.NewGuid().ToString()
            });

            //Act and Assert
            var productBatchService = new ProductBatchService(queueServiceMock.Object, extractionServiceMock.Object,
                fileRepositoryMock.Object, productRepositoryMock.Object);

            await Assert.ThrowsAsync<InvalidDataException>(() => productBatchService.ProcessFile());
        }
    }
}
