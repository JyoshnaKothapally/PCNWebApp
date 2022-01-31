using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PCNApi.Data.Services;
using PCNApi.Models;

namespace PCNApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductsService productsService, ILogger<ProductsController> logger)
        {
            _productsService = productsService;
            _logger = logger;
        }

        [HttpGet("GetAllProductsCount")]
        public async Task<IActionResult> GetAllProductsCount()
        {
            try
            {
                _logger.LogInformation("This is just a log in GetAllProducts()");
                var result = await _productsService.GetAllProductsCount();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Sorry, we could not load the Products");
            }
        }

        [HttpGet("GetPaginatedProducts/{pageNumber}/{pagesize}")]
        public async Task<IActionResult> GetPaginatedProducts(int pageNumber,int pagesize)
        {
            try
            {
                _logger.LogInformation("This is just a log in GetAllProducts()");
                var result = await _productsService.GetPaginatedProducts(pageNumber,pagesize);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Sorry, we could not load the Products");
            }
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await _productsService.GetProductById(id);

            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpDelete("DeleteProductById/{id}")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            try
            {
                await _productsService.DeleteProductById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateProduct/{product}")]
        public async Task<IActionResult> UpdateProduct(Product p)
        {
            try
            {
                await _productsService.UpdateProduct(p);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
