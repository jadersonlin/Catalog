using Azure.Storage.Queues;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

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

        public async Task<Message> DequeueMessage()
        {
            var queueClient = await GetQueueClient();
            var response = await queueClient.ReceiveMessageAsync();

            if (response.Value == null)
                return null;

            return new Message
            {
                MessageId = response.Value.MessageId,
                MessageText = response.Value.MessageText,
                PopReceipt = response.Value.PopReceipt
            };
        }

        public async Task DeleteMessage(Message message)
        {
            var queueClient = await GetQueueClient();
            await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }
    }
}
