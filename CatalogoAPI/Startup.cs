using Catalog.Application.Impl;
using Catalog.Application.Interfaces;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Messages;
using Catalog.Infrastructure.MongoDb;
using Catalog.Infrastructure.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Catalog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
                c.IncludeXmlComments(GetXmlCommentsFilePath());
            });

            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration.GetSection("Storage:ConnectionString").Value);
                builder.AddQueueServiceClient(Configuration.GetSection("Queue:ConnectionString").Value);
            });

            services.AddScoped<IStorageService, FileStorageService>();
            services.AddScoped<IQueueService, AzureQueueService>();
            services.AddScoped<ICatalogContext, CatalogMongoDbContext>();
            services.AddScoped<IFileRepository, FileMongoDbRepository>();
            services.AddScoped<IProductRepository, ProductMongoDbRepository>();
            services.AddScoped<ICatalogService, CatalogService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static string GetXmlCommentsFilePath()
        {
            var basePath = AppContext.BaseDirectory;
            var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }
}
