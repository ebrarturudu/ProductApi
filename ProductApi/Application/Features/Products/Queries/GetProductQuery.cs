using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Persistence;
using System.Text.Json;

namespace ProductApi.Application.Features.Products.Queries;

public record GetProductsQuery() : IRequest<IEnumerable<Product>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;

    public GetProductsQueryHandler(AppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = "productList";

        var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<List<Product>>(cachedData)!;
        }

        var products = await _context.Products.ToListAsync(cancellationToken);

        var serializedData = JsonSerializer.Serialize(products);
        await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        }, cancellationToken);

        return products;
    }
}