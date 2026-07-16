using MediatR;
using MyShop.WebApi.Applications.Queries.ProductQueries;
using MyShop.WebApi.Domain.Dto;
using MyShop.WebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Common.Exceptions;
using MyShop.WebApi.Domain.Entities;

namespace MyShop.WebApi.Applications.Handlers.ProductHandlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly AppDbContext _context;

        public GetProductByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id, cancellationToken);
            if (product == null)
                throw new NotFoundException(nameof(Product),request.Id);

            return new ProductDto()
            {
                Id = product.Id,
                Category = product.ProductCategory.ToString(),
                Description = product.ProductDescription,
                Name = product.ProductName,
                Price = product.ProductPrice,
                ImageUrl = product.ImageUrl
            };
        }
    }
}
