using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Catalog.API;
using Catalog.API.Controllers;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Catalog.Tests.Api
{
    public class FilesControllerTests
    {
        private readonly Mock<IStorageService> storageServiceMock;

        public FilesControllerTests()
        {
            storageServiceMock = new Mock<IStorageService>();
        }

        [Fact]
        public async Task POST_can_insert_spreadsheet()
        {
            //Arrange
            const string fileName = "products_test_no_errors.xlsx";
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\TestFiles\\" + fileName;
            var stream = File.OpenRead(path);
            stream.Position = 0;
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);
            
            var uploadFileResult = new UploadFileResult
            {
                Id = Guid.NewGuid().ToString(),
                FileName = fileName,
                Length = 10000,
                UploadedAt = new DateTime(2020,01,01),
                Message = "File uploaded"
            };
            
            storageServiceMock.Setup(_ => _.Upload(fileMock.Object)).ReturnsAsync(uploadFileResult);
            
            //Act
            var controller = new FilesController(storageServiceMock.Object);
            var result = await controller.Upload(fileMock.Object);
            var actualResultObj = ((OkObjectResult)result.Result).Value;
            var actualResult = actualResultObj as UploadFileResult;

            //assert
            Assert.IsAssignableFrom<UploadFileResult>(actualResultObj);
            Assert.IsAssignableFrom<UploadFileResult>(actualResult);
            Assert.IsType<DateTime>(actualResult.UploadedAt);
            Assert.IsType<string>(actualResult.FileName);
            Assert.IsType<string>(actualResult.Message);

            var isIdValidGuid = Guid.TryParse(actualResult.Id, out _);
            Assert.True(isIdValidGuid);
        }

        [Fact]
        public async Task GET_can_get_processing_status()
        {
            //Arrange
            var guid = Guid.NewGuid().ToString();

            var expectedProcessingStatus = new GetProcessingStatusResult
            {
                Id = guid,
                UploadedAt = DateTime.Now,
                ProcessedAt = DateTime.Now,
                FileName = "file.xlsx",
                Length = 100,
                Message = "message",
                Status = "Processed"
            };

            storageServiceMock.Setup(_ => _.GetProcessingStatus(guid)).ReturnsAsync(expectedProcessingStatus);

            //Act
            var controller = new FilesController(storageServiceMock.Object);
            var result = await controller.GetStatus(guid);
            var actualResultObj = ((OkObjectResult)result.Result).Value;
            var actualResult = actualResultObj as GetProcessingStatusResult;

            //Assert
            Assert.IsAssignableFrom<GetProcessingStatusResult>(actualResultObj);
            Assert.IsAssignableFrom<GetProcessingStatusResult>(actualResult);
            Assert.Equal("Processed", actualResult.Status);
            Assert.True(actualResult.Length > 0);
            var isIdValidGuid = Guid.TryParse(actualResult.Id, out _);
            Assert.True(isIdValidGuid);
            Assert.Contains(".xlsx", actualResult.FileName);
        }
    }
}
