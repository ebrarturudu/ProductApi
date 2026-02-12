using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Core.Entities;              
using ProductApi.Application.DTOs;           
using ProductApi.Infrastructure.Persistence; 
using Microsoft.Extensions.Caching.Distributed;
using MediatR;
using ProductApi.Application.Features.Products.Queries;
using ProductApi.Application.Features.Products.Commands;

namespace ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache; 
    private readonly IMediator _mediator;

    //public ProductsController(AppDbContext context, IDistributedCache cache)
    //{
      //  _context = context;
      //  _cache = cache;
    //}

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
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound("Ürün bulunamadı!");

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound("Silinecek ürün bulunamadı!");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        await _cache.RemoveAsync("productList");

        return NoContent(); 
    }
}
