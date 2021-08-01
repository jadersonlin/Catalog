using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    /// <summary>
    /// Actions in Product Context
    /// </summary>
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICatalogService catalogService;

        /// <summary>
        /// Actions in Product Context
        /// </summary>
        /// <param name="catalogService"></param>
        public ProductsController(ICatalogService catalogService)
        {
            this.catalogService = catalogService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lm"></param>
        /// <returns></returns>
        [Route("{lm}")]
        [HttpGet]
        public async Task<ActionResult<GetProductResult>> Get(int lm)
        {
            var product = await catalogService.GetProduct(lm);

            if (product != null)
                return Ok(product);

            return NotFound("Product not found!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="lm"></param>
        /// <returns></returns>
        [Route("{lm}")]
        [HttpPut]
        public async Task<ActionResult> Put([FromBody]PutProductInput input, int lm)
        {
            var result = await catalogService.EditProduct(input, lm);

            if (result)
                return Ok("Sucess!");

            return NotFound("Product not found to edit!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lm"></param>
        /// <returns></returns>
        [Route("{lm}")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int lm)
        {
            var result = await catalogService.DeleteProduct(lm);

            if (result)
                return Ok("Success!");

            return NotFound("Product not found to delete!");
        }
    }
}
