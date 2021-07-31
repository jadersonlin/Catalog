using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Application.Dtos;

namespace Catalog.Application.Interfaces
{
    public interface IExtractionService
    {
        Task<Tuple<IList<ProductData>, IList<KeyValuePair<string, string>>>> ExtractDataFromFile(string fileId);
    }
}
