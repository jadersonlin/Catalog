using System;
using System.Threading.Tasks;
using Catalog.Application.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    /// <summary>
    /// Actions in Product Context
    /// </summary>
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> logger;
        private readonly IWebHostEnvironment environment;

        /// <summary>
        /// Actions in Product Context
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        public ProductsController(ILogger<ProductsController> logger, 
            IWebHostEnvironment environment)
        {
            this.logger = logger;
            this.environment = environment;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Route("~/[action]/{id}")]
        [HttpDelete]
        public async Task<DeleteProductResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
