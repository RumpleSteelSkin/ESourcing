using ESourcing.Products.Entities;
using ESourcing.Products.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.Products.Controller;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductRepository productRepository, ILogger<ProductController> logger) : ControllerBase
{
    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        return Ok(await productRepository.GetAllAsync());
    }

    [HttpGet("Get/{id:length(24)}", Name = "Get")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> Get(string id)
    {
        var product = await productRepository.GetAsync(id);
        if (product is not null) return Ok(product);
        logger.LogError("Product with id {Id} was not found", id);
        return NotFound();
    }

    [HttpPost("Create")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    public async Task<ActionResult<Product>> Create([FromBody] Product product)
    {
        await productRepository.Create(product);
        return CreatedAtRoute("Get", new { id = product.Id }, product);
    }

    [HttpPut("Update")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public async Task<ActionResult<Product>> Update([FromBody] Product product)
    {
        await productRepository.Update(product);
        return Ok(product);
    }

    [HttpDelete("Delete/{id:length(24)}", Name = "Delete")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(string id)
    {
        return Ok(await productRepository.Delete(id));
    }
}