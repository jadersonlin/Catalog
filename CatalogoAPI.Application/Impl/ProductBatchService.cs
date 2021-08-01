using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Domain.Enums;
using Catalog.Domain.Models;
using Catalog.Domain.Repositories;
using Newtonsoft.Json;
using File = Catalog.Domain.Models.File;

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

        public async Task<FileProcessingStatus?> ProcessFile()
        { 
            var message = await queueService.DequeueMessage();

            if (message == null)
                return null;

            var file = GetFileData(message.MessageText);

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

            return fileModel.Status;
        }

        private UploadFileResult GetFileData(string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText))
                throw new InvalidDataException("Message content is invalid.");

            UploadFileResult file;

            try
            {
                file = JsonConvert.DeserializeObject<UploadFileResult>(messageText);
            }
            catch (JsonReaderException ex)
            {
                throw new InvalidDataException("Invalid message format.", ex);
            }

            return file;
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
