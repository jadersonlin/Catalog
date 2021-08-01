using Catalog.Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace Catalog.Infrastructure.Storage
{
    public class FileHandler
    {
        private readonly IConfiguration configuration;
        private readonly BlobServiceClient blobServiceClient;
        private const string ValidExtension = ".xlsx";
        private readonly BlobHandler blobHandler;

        public FileHandler(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            this.configuration = configuration;
            this.blobServiceClient = blobServiceClient;
            blobHandler = new BlobHandler(configuration, blobServiceClient);
        }

        public async Task<FileData> SaveFile(IFormFile file)
        {
            if (!IsFileValid(file))
                throw new InvalidDataException("Arquivo inválido!");

            var id = Guid.NewGuid();

            var fileName = id + ValidExtension;

            await using var stream = file.OpenReadStream();

            var blobClient = blobHandler.GetBlobClient(fileName);
            var result = await blobClient.UploadAsync(stream, false);
            var hash = Convert.ToBase64String(result.Value.ContentHash);

            return GetFileData(file, id, hash);
        }
        
        private bool IsFileValid(IFormFile file)
        {
            return IsLenghtValid(file) && IsExtensionValid(file);
        }

        private bool IsLenghtValid(IFormFile file)
        {
            return file.Length > 0;
        }

        private bool IsExtensionValid(IFormFile file)
        {
            return Path.GetExtension(file.FileName) == ValidExtension;
        }

        private FileData GetFileData(IFormFile file, Guid id, string hash)
        {
            return new FileData
            {
                Id = id.ToString(),
                FileName = file.FileName,
                Length = file.Length,
                UploadedAt = DateTime.Now,
                Hash = hash
            };
        }

        public async Task<Stream> DownloadFile(string fileId)
        {
            var blobClient = blobHandler.GetBlobClient(fileId + ValidExtension);

            var file = await blobClient.DownloadAsync();

            return file.Value.Content;
        }
    }
}
