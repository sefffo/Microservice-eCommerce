using Azure;
using Ecommerce.SherdLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTO_s;
using ProductApi.Application.DTO_s.Conversions;
using ProductApi.Application.Interfaces;
using Response = Ecommerce.SherdLibrary.Responses.Response;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct productInterface) : ControllerBase
    {
        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var result = await productInterface.GetAllAsync();

            if (!result.Any())
            {
                return NotFound("No products found in the DB");
            }
            var products = ProductConversion.ToProductDtoList(result);
            if (!products.Any())
            { 
                return NotFound("No products found after conversion");
            }

            return Ok(products);
        }
        // GET: /api/Product/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var result = await productInterface.FindByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            var productDto = result.ToProductDto();
            return Ok(productDto);
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByName([FromQuery] string name)
        //{
        //    var result = await productInterface.FindByNameAsync(name);
        //    if (result == null || !result.Any())
        //    {
        //        return NotFound($"No products found with the name '{name}'.");
        //    }
        //    var productsDto = ProductConversion.ToProductDtoList(result);
        //    return Ok(productsDto);
        //}

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDto productDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //convert from dto to entity
            var productEntity = productDto.ToProductEntity();
            if(productEntity is null )
            {
                return BadRequest("Conversion from DTO to Entity failed.");
            }
            var response = await productInterface.CreateAsync(productEntity);
            if (response.flag is false) 
            {
                return BadRequest(response.message);
            }
            return Ok(response);
        }
        // PUT: /api/Product/5
        [HttpPut("{id:int}")]
 
        public async Task<ActionResult<Response>> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate ID matches
            if (id != productDto.Id)
            {
                return BadRequest("Product ID mismatch between URL and body");
            }

            // Convert DTO to entity
            var productEntity = productDto.ToProductEntity();

            // Call repository
            var response = await productInterface.UpdateAsync(productEntity);

            if (!response.flag)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        // DELETE: api/Product/5
        public async Task<ActionResult<Response>> DeleteProduct(int id)
        {
            var product = await productInterface.FindByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            var response = await productInterface.DeleteAsync(product);

            if (!response.flag)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
