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
        private readonly IFormFile file;
        private readonly BlobServiceClient blobServiceClient;
        private const string ValidExtension = ".xlsx";

        public FileHandler(IConfiguration configuration, IFormFile file, BlobServiceClient blobServiceClient)
        {
            this.configuration = configuration;
            this.file = file;
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<FileData> SaveFile()
        {
            if (!IsValid())
                throw new InvalidDataException("Arquivo inválido!");

            var id = Guid.NewGuid();

            var fileName = id + ValidExtension;

            await using var stream = file.OpenReadStream();

            var blobClient = GetBlobClient(fileName);
            var result = await blobClient.UploadAsync(stream, false);

            var hash = Convert.ToBase64String(result.Value.ContentHash);

            return GetFileData(id, hash);
        }

        private BlobClient GetBlobClient(string blobName)
        {
            var containerClient = GetContainerClient();

            if (!containerClient.Exists()) 
                throw new InvalidOperationException("Blob client doesn't exist.");

            var blobClient = containerClient.GetBlobClient(blobName);
             
            return blobClient;
        }

        private BlobContainerClient GetContainerClient()
        {
            var containerName = configuration.GetSection("Storage:ContainerName").Value;
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();
            return containerClient;
        }

        private bool IsValid()
        {
            return file.Length > 0 && Path.GetExtension(file.FileName) == ValidExtension;
        }

        private FileData GetFileData(Guid id, string hash)
        {
            return new FileData
            {
                Id = id,
                FileName = file.FileName,
                Length = file.Length,
                UploadedAt = DateTime.Now,
                Hash = hash
            };
        }
    }
}
