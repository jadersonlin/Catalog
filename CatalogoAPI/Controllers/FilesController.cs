using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IStorageService storageService;
        
        public FilesController(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload")]
        public async Task<UploadFileResult> Upload([FromForm] IFormFile file)
        {
            return await storageService.Upload(file);
        }
    }
}
