using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;

namespace Catalog.Infrastructure.Storage
{
    public class BlobHandler
    {
        private readonly IConfiguration configuration;
        private readonly BlobServiceClient blobServiceClient;

        public BlobHandler(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            this.configuration = configuration;
            this.blobServiceClient = blobServiceClient;
        }

        public BlobClient GetBlobClient(string blobName)
        {
            var containerClient = GetContainerClient();

            if (!containerClient.Exists())
                throw new InvalidOperationException("Blob client doesn't exist.");

            var blobClient = containerClient.GetBlobClient(blobName);

            return blobClient;
        }

        public BlobContainerClient GetContainerClient()
        {
            var containerName = configuration.GetSection("Storage:ContainerName").Value;
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();
            return containerClient;
        }
    }
}
