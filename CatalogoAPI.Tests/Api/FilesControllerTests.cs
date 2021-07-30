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
            const string fileName = "products_test_no_errors.xlsx";
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\TestFiles\\" + fileName;
            var stream = File.OpenRead(path);
            stream.Position = 0;
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(stream.Length);
            var uploadFileResultMock = new Mock<UploadFileResult>();
            uploadFileResultMock.SetupAllProperties();
            storageServiceMock.Setup(_ => _.Upload(fileMock.Object)).ReturnsAsync(uploadFileResultMock.Object);
            
            var controller = new FilesController(storageServiceMock.Object);
            var result = await controller.Upload(fileMock.Object);

            Assert.IsAssignableFrom<UploadFileResult>(result);
            Assert.IsType<Guid>(result.Id);
            Assert.IsType<DateTime>(result.UploadedAt);
        }
    }
}
