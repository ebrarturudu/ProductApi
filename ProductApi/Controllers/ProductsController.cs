using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Core.Entities;              
using ProductApi.Application.DTOs;           
using ProductApi.Infrastructure.Persistence; 
using Microsoft.Extensions.Caching.Distributed;
using MediatR;
using ProductApi.Application.Features.Products.Queries;
using ProductApi.Application.Features.Products.Commands;
using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

   [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
    var products = await _mediator.Send(new GetProductsQuery());
    return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(ProductDto productDto)
    {
        var result = await _mediator.Send(new CreateProductCommand(productDto));
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        return product != null ? Ok(product) : NotFound("Ürün bulunamadı.");
    }

   [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));
        if (!result) return NotFound("Silinecek ürün bulunamadı.");
        return NoContent(); 
    }
}
