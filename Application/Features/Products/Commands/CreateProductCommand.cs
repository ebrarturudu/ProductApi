using MediatR;
using ProductApi.Application.DTOs;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Persistence;
using Microsoft.Extensions.Caching.Distributed;

namespace ProductApi.Application.Features.Products.Commands;

public record CreateProductCommand(ProductDto ProductDto) : IRequest<Product>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;

    public CreateProductCommandHandler(AppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.ProductDto.Name,
            Price = request.ProductDto.Price,
            Stock = request.ProductDto.Stock
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync("productList", cancellationToken);

        return product;
    }
}