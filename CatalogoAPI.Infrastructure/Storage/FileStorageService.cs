using Azure.Storage.Blobs;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Storage
{
    public class FileStorageService : IStorageService
    {
        private readonly IConfiguration configuration;
        private readonly IQueueService queueService;
        private const string SaveFileMessage = "File successfully uploaded. Use Id to get the processing status.";
        private readonly FileHandler fileHandler;

        public FileStorageService(IConfiguration configuration,
                                  IQueueService queueService,
                                  BlobServiceClient blobServiceClient)
        {
            this.configuration = configuration;
            this.queueService = queueService;
            fileHandler = new FileHandler(configuration, blobServiceClient);
        }

        public async Task<UploadFileResult>Upload(IFormFile file)
        {
            var fileData = await fileHandler.SaveFile(file);

            var result = new UploadFileResult
            {
                Id = fileData.Id,
                Length = fileData.Length,
                FileName = fileData.FileName,
                UploadedAt = fileData.UploadedAt,
                Message = SaveFileMessage
            };

            var jsonResult = JsonConvert.SerializeObject(result);

            await queueService.SendMessage(jsonResult);

            return result;
        }

        public async Task<Stream> GetFile(string fileId)
        { 
            return await fileHandler.DownloadFile(fileId);
        }
    }
}
