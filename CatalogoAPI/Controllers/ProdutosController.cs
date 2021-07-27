using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CatalogoAPI.Models;

namespace CatalogoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ILogger<ProdutosController> logger;

        public ProdutosController(ILogger<ProdutosController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<UploadFileResult> Upload([FromForm] IFormFile file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<GetProductResult> Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Route("~/[action]/{id}")]
        [HttpPut]
        public async Task<PutProductResult> Put(PutProductInput input, int id)
        {
            throw new NotImplementedException();
        }
    }
}
