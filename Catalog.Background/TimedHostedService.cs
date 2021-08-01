using Catalog.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Background
{
    public class TimedHostedService : IHostedService, IDisposable
    {

        private readonly IProductBatchService productBatchService;
        private readonly ILogger<TimedHostedService> logger;
        private int executionCount = 0;
        private Timer timer;

        public TimedHostedService(IServiceScopeFactory factory,
            ILogger<TimedHostedService> logger)
        {
            this.logger = logger;
            productBatchService = factory.CreateScope().ServiceProvider.GetRequiredService<IProductBatchService>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service running.");

            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            await productBatchService.ProcessFile();

            logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service is stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
