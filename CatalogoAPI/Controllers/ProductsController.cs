using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Catalog.API.Controllers
{
    /// <summary>
    /// Actions in Product Context
    /// </summary>
    [Route("api/products")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly ICatalogService catalogService;

        public ProductsController(ICatalogService catalogService)
        {
            this.catalogService = catalogService;
        }

        /// <summary>
        /// Get products by Lm.
        /// </summary>
        /// <param name="lm">Product Id</param>
        /// <returns>Product data</returns>
        [Route("{lm}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetProductResult>> Get(int lm)
        {
            var product = await catalogService.GetProduct(lm);

            if (product != null)
                return Ok(product);

            return NotFound("Product not found!");
        }

        /// <summary>
        /// Edit product data.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/products/1
        /// 
        ///     {
        ///         "name": "test product",
        ///         "freeShipping": false,
        ///         "description": "test description",
        ///         "price": 10.0,
        ///         "categoryId": 123
        ///     }
        ///
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="lm">Product Id</param>
        /// <returns>Action result</returns>
        [Route("{lm}")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put([FromBody]PutProductInput input, int lm)
        {
            var result = await catalogService.EditProduct(input, lm);

            if (result)
                return Ok("Sucess!");

            return NotFound("Product not found to edit!");
        }

        /// <summary>
        /// Remove a product.
        /// </summary>
        /// <param name="lm">Product Id</param>
        /// <returns>Action result</returns>
        [Route("{lm}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int lm)
        {
            var result = await catalogService.DeleteProduct(lm);

            if (result)
                return Ok("Success!");

            return NotFound("Product not found to delete!");
        }
    }
}
