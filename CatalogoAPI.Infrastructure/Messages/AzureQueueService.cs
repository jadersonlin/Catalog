using System;
using System.Threading.Tasks;
using Azure;
using Catalog.Application.Interfaces;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;

namespace Catalog.Infrastructure.Messages
{
    public class AzureQueueService : IQueueService
    {
        private readonly IConfiguration configuration;
        private readonly QueueServiceClient queueServiceClient;

        public AzureQueueService(IConfiguration configuration,
            QueueServiceClient queueServiceClient)
        {
            this.configuration = configuration;
            this.queueServiceClient = queueServiceClient;
        }

        public async Task<string> SendMessage(string message)
        {
            var queueClient = await GetQueueClient();
            var result = await queueClient.SendMessageAsync(message);

            return result.Value.MessageId;
        }

        private async Task<QueueClient> GetQueueClient()
        {
            var queueName = configuration.GetSection("Queue:QueueName").Value;
            var queueClient = queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync();

            if (!queueClient.Exists())
                throw new InvalidOperationException("Queue client doesn't exist.");

            return queueClient;
        }

        public async Task<QueueMessage> DequeueMessage()
        {
            var queueClient = await GetQueueClient();
            return await queueClient.ReceiveMessageAsync();
        }

        public async Task<Response> DeleteMessage(QueueMessage message)
        {
            var queueClient = await GetQueueClient();
            return await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }
    }
}
