using System.Net;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
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
        public async Task<ActionResult<UploadFileResult>> Upload([FromForm] IFormFile file)
        {
            var result = await storageService.Upload(file);

            if (result != null)
                return Ok(result);

            return NotFound("File was not uploaded.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("upload/status/{fileId}")]
        public async Task<ActionResult<GetProcessingStatusResult>> GetStatus(string fileId)
        {
            var status = await storageService.GetProcessingStatus(fileId);

            if (status != null)
                return Ok(status);

            return NotFound($"Status not found for file {fileId}");
        }
    }
}
