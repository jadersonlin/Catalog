using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Catalog.Infrastructure.Storage
{
    public class FileStorageService : IStorageService
    {
        public async Task<UploadFileResult> UploadFile(IFormFile file, string rootPath)
        {
            var fileHandler = new FileHandler(file, rootPath);

            var fileGuid = await fileHandler.SaveFile();

            return new UploadFileResult
            {
                FileName = file.FileName,
                Id = fileGuid,
                Message = "Arquivo incluído com sucesso."
            };
        }
    }
}
