using System.IO;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Interfaces
{
    public interface IStorageService
    {
        Task<UploadFileResult> Upload(IFormFile file);
        Task<Stream> GetFile(string fileId);
        Task<GetProcessingStatusResult> GetProcessingStatus(string fileId);
    }
}
