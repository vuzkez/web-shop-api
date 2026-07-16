using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Commands.ProductCommands;
using MyShop.WebApi.Applications.Common.Exceptions;
using MyShop.WebApi.Domain.Entities;
using MyShop.WebApi.Infrastructure.Data;

namespace MyShop.WebApi.Applications.Handlers.ProductHandlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateProductCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            var product = await _context.Products.FindAsync(request.Id, ct);
            if (product == null) 
                throw new NotFoundException(nameof(Product), request.Id);

            if (request.Name != null) product.ProductName = request.Name;
            if (request.Price.HasValue) product.ProductPrice = request.Price.Value;
            if (request.Description != null) product.ProductDescription = request.Description;
            if (request.Category.HasValue) product.ProductCategory = request.Category.Value;
            if (request.ImageUrl != null) product.ImageUrl = request.ImageUrl;

            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
