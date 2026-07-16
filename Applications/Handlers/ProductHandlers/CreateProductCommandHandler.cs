using MediatR;
using MyShop.WebApi.Applications.Commands.ProductCommands;
using MyShop.WebApi.Infrastructure.Data;
using MyShop.WebApi.Domain.Entities;

namespace MyShop.WebApi.Applications.Handlers.ProductHandlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly AppDbContext _context;

        public CreateProductCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                ProductName = request.Name,
                ProductDescription = request.Description,
                ProductCategory = request.Category,
                ProductPrice = request.Price,
                ImageUrl = request.ImageUrl
            };
            await _context.Products.AddAsync(product,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }
}
