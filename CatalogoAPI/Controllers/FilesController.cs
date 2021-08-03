using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    /// <summary>
    /// Actions in File Context
    /// </summary>
    [Route("api/files")]
    [ApiController]
    [Produces("application/json")]
    public class FilesController : ControllerBase
    {
        private readonly IStorageService storageService;
        
        public FilesController(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        /// <summary>
        /// Upload a spreadsheet to insert a batch of products.
        /// </summary>
        /// <param name="file">XLSX spreadsheet, sent via form-data interface</param>
        /// <returns>Uploaded file data</returns>
        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UploadFileResult>> Upload([FromForm] IFormFile file)
        {
            var result = await storageService.Upload(file);

            if (result != null)
                return Ok(result);

            return NotFound("File was not uploaded.");
        }

        /// <summary>
        /// Get processing status from uploaded spreadsheet.
        /// </summary>
        /// <param name="fileId">File identificator</param>
        /// <returns>Uploaded file data and processing status</returns>
        [HttpGet]
        [Route("upload/status/{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetProcessingStatusResult>> GetStatus(string fileId)
        {
            var status = await storageService.GetProcessingStatus(fileId);

            if (status != null)
                return Ok(status);

            return NotFound($"Status not found for file {fileId}");
        }
    }
}
