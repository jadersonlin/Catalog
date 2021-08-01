using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Domain.Enums;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories;
using Newtonsoft.Json;

namespace Catalog.Application.Impl
{
    public class ProductBatchService : IProductBatchService
    {
        private readonly IQueueService queueService;
        private readonly IExtractionService extractionService;
        private readonly IFileRepository fileRepository;
        private readonly IProductRepository productRepository;

        public ProductBatchService(IQueueService queueService,
                                   IExtractionService extractionService,
                                   IFileRepository fileRepository,
                                   IProductRepository productRepository)
        {
            this.queueService = queueService;
            this.extractionService = extractionService;
            this.fileRepository = fileRepository;
            this.productRepository = productRepository;
        }

        public async Task ProcessFile()
        { 
            var message = await queueService.DequeueMessage();

            if (message == null)
                return;

            var file = JsonConvert.DeserializeObject<UploadFileResult>(message.MessageText);

            if (file == null)
                throw new InvalidOperationException("Message File not found.");

            var (products, validationErrors) = await extractionService.ExtractDataFromFile(file.Id);

            var fileModel = MapFile(file);
            fileModel.ValidationErrors = validationErrors;

            await fileRepository.Insert(fileModel);

            if (fileModel.Status == FileProcessingStatus.Processed)
            {
                var productModels = MapProductModels(products);
                await productRepository.InsertMany(productModels);
            }

            await queueService.DeleteMessage(message);
        }

        private File MapFile(UploadFileResult file)
        {
            return new File(file.Id)
            {
                FileName = file.FileName,
                UploadedAt = file.UploadedAt,
                ProcessedAt = DateTime.Now,
                Length = file.Length
            };
        }

        private IList<Product> MapProductModels(IList<ProductData> products)
        {
            return products.Select(p => new Product(p.Lm, p.Name, p.FreeShipping, p.Description, p.Price, p.CategoryId)).ToList();
        }
    }
}
