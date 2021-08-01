using Azure.Storage.Blobs;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Catalog.Domain.Repositories;
using File = Catalog.Domain.Models.File;

namespace Catalog.Infrastructure.Storage
{
    public class FileStorageService : IStorageService
    {
        private readonly IQueueService queueService;
        private readonly IFileRepository fileRepository;
        private const string SaveFileMessage = "File successfully uploaded. Use Id to get the processing status.";
        private readonly FileHandler fileHandler;

        public FileStorageService(IConfiguration configuration,
                                  IQueueService queueService,
                                  BlobServiceClient blobServiceClient,
                                  IFileRepository fileRepository)
        {
            this.queueService = queueService;
            this.fileRepository = fileRepository;
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

        public async Task<GetProcessingStatusResult> GetProcessingStatus(string fileId)
        {
            var file = await fileRepository.GetById(fileId);

            return MapFile(file);
        }

        private GetProcessingStatusResult MapFile(File file)
        {
            if (file == null)
                return null;

            return new GetProcessingStatusResult
            {
                Id = file.Id,
                FileName = file.FileName,
                UploadedAt = file.UploadedAt,
                ProcessedAt = file.ProcessedAt,
                ValidationErrors = file.ValidationErrors,
                Length = file.Length,
                Status = file.Status.ToString(),
                Message = "The status determines if file was processed with success."
            };
        }
    }
}
