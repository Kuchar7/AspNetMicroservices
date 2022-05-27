using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/catalog")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("products")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetProducts());
        }

        [HttpGet("products/{id}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct([FromRoute] string id)
        {
            var product = await _productRepository.GetProduct(id);
            if(product == null)
            {
                _logger.LogError($"Not found product with id {id}");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("products/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory([FromRoute] string category)
        {
            return Ok(await _productRepository.GetProductsByCategory(category));
        }

        [HttpGet("products/{name}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName([FromRoute] string name)
        {
            return Ok(await _productRepository.GetProductsByCategory(name));
        }

        [HttpPost("products")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);
            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }

        [HttpPut("products/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]

        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            await _productRepository.UpdateProduct(product);
            return NoContent();
        }

        [HttpDelete("products/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteProduct([FromRoute] string id)
        {
            await _productRepository.DeleteProduct(id);
            return NoContent();
        }
    }
}
