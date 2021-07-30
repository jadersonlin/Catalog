using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Catalog.Infrastructure.Storage
{
    public class FileStorageService : IStorageService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private readonly IQueueService queueService;
        private readonly BlobServiceClient blobServiceClient;
        private const string SaveFileMessage = "File successfully uploaded. Use Id to get the processing status.";

        public FileStorageService(IConfiguration configuration,
                                  ILogger logger,
                                  IQueueService queueService,
                                  BlobServiceClient blobServiceClient)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.queueService = queueService;
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<UploadFileResult>Upload(IFormFile file)
        {
            var fileHandler = new FileHandler(configuration, file, blobServiceClient);

            var fileData = await fileHandler.SaveFile();

            var result = new UploadFileResult
            {
                Id = fileData.Id,
                Length = fileData.Length,
                FileName = fileData.FileName,
                UploadedAt = fileData.UploadedAt,
                Message = SaveFileMessage
            };

            var jsonResult = JsonConvert.SerializeObject(result);

            var messageId = await queueService.SendMessage(jsonResult);

            logger.LogInformation(messageId, "New file uploaded.", jsonResult);

            return result;
        }
    }
}
