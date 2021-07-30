using System;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Catalog.Domain.Repositories;

namespace Catalog.Application.Impl
{
    public class ProductBatchService : IProductBatchService
    {
        public ProductBatchService(IQueueService queueService,
                                   IStorageService storageService,
                                   IProductRepository productRepository)
        {
            
        }

        public Task<ProcessFileResult> ProcessFile(ProcessFileInput input)
        {
            throw new NotImplementedException();
        }
    }
}
