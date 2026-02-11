using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Core.Entities;              
using ProductApi.Application.DTOs;           
using ProductApi.Infrastructure.Persistence; 
using Microsoft.Extensions.Caching.Distributed;

namespace ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache; 

    public ProductsController(AppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

   [HttpGet]
public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
{
    string cacheKey = "productList";
    
    var cachedData = await _cache.GetStringAsync(cacheKey);
    if (!string.IsNullOrEmpty(cachedData))
    {
        var products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(cachedData);
        return Ok(products);
    }

    var productsFromDb = await _context.Products.ToListAsync();

    var serializedData = System.Text.Json.JsonSerializer.Serialize(productsFromDb);
    var cacheOptions = new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // 10 dakika sakla
    };
    
    await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);

    return Ok(productsFromDb);
}

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Stock = productDto.Stock
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        await _cache.RemoveAsync("productList"); 

        return Ok(product);
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
