using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Queries.ProductQueries;
using MyShop.WebApi.Domain.Dto;
using MyShop.WebApi.Infrastructure.Data;

namespace MyShop.WebApi.Applications.Handlers.ProductsHandlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery,List<ProductDto>>
    {
        private readonly AppDbContext _context;

        public GetProductsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.ProductName,
                    Price = p.ProductPrice,
                    Description = p.ProductDescription,
                    Category = p.ProductCategory.ToString(),
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync(cancellationToken);
        }
    }
}
