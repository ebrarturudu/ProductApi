using MediatR;
using ProductApi.Application.DTOs;
using ProductApi.Core.Entities;
using ProductApi.Infrastructure.Persistence;
using Microsoft.Extensions.Caching.Distributed;

namespace ProductApi.Application.Features.Products.Commands;
public record DeleteProductCommand(int Id) : IRequest<bool>;
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;

    public DeleteProductCommandHandler(AppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync("productList", cancellationToken);
        await _cache.RemoveAsync($"product_{request.Id}", cancellationToken);
        
        return true;
    }
}