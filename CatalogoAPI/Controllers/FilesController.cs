using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<ProductsController> logger;
        private readonly IWebHostEnvironment environment;
        private readonly IStorageService storageService;

        public FilesController(ILogger<ProductsController> logger,
            IWebHostEnvironment environment,
            IStorageService storageService)
        {
            this.logger = logger;
            this.environment = environment;
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
            return await storageService.UploadFile(file, environment.WebRootPath);
        }
    }
}
