using Catalog.Application.Impl;
using Catalog.Application.Interfaces;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Extraction;
using Catalog.Infrastructure.Messages;
using Catalog.Infrastructure.MongoDb;
using Catalog.Infrastructure.Storage;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog.Background
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    services.AddScoped<IQueueService, AzureQueueService>();
                    services.AddScoped<ICatalogContext, CatalogMongoDbContext>();
                    services.AddScoped<IProductBatchService, ProductBatchService>();
                    services.AddScoped<IExtractionService, ExcelExtractionService>();
                    services.AddScoped<IStorageService, FileStorageService>();
                    services.AddScoped<IFileRepository, FileMongoDbRepository>();
                    services.AddScoped<IProductRepository, ProductMongoDbRepository>();
                    services.AddHostedService<TimedHostedService>();
                    services.AddAzureClients(builder =>
                    {
                        builder.AddBlobServiceClient(configuration.GetSection("Storage:ConnectionString").Value);
                        builder.AddQueueServiceClient(configuration.GetSection("Queue:ConnectionString").Value);
                    });
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                });
    }
}
