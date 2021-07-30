using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Interfaces
{
    public interface IStorageService
    {
        Task<UploadFileResult> Upload(IFormFile file);
    }
}
