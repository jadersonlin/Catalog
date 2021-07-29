using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Newtonsoft.Json;
using Xunit;

namespace Catalog.Tests.Api
{
    public class FilesControllerTests
    {
        private readonly IStorageService storageService;
        private readonly HttpClient httpClient;

        public FilesControllerTests(IStorageService storageService)
        {
            this.storageService = storageService;
            httpClient = new HttpClient();
        }

        [Fact]
        public async Task POST_can_insert_spreadsheet()
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var stream = File.OpenRead(path);
            HttpContent fileStreamContent = new StreamContent(stream);

            using var formData = new MultipartFormDataContent
            {
                { fileStreamContent, "file" }
            };

            var response = await httpClient.PostAsync("api/products/upload", formData);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UploadFileResult>(responseBody);

            Assert.NotNull(result);
            Assert.NotNull(result.Id);
        }
    }
}
