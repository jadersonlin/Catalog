using System;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;

namespace Catalog.Application.Impl
{
    class ProductBatchService : IProductBatchService
    {
        public Task<ProcessFileResult> ProcessFile(ProcessFileInput input)
        {
            throw new NotImplementedException();
        }
    }
}
