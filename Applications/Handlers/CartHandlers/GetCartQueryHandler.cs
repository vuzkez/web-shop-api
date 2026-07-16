using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Queries.CartQueries;
using MyShop.WebApi.Domain.Dto;
using MyShop.WebApi.Infrastructure.Data;

namespace MyShop.WebApi.Applications.Handlers.CartHandlers
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartDto>
    {
        private readonly AppDbContext _context;

        public GetCartQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CartDto> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(i => i.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(i => i.UserId == request.UserId, cancellationToken);

            if (cart == null)
                return new CartDto { Items = new List<CartItemDto>() };

            return new CartDto
            {
                Id = cart.Id,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.ProductName,
                    ProductImage = i.Product.ImageUrl,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.Quantity * i.UnitPrice
                }).ToList(),
                TotalPrice = cart.Items.Sum(i => i.Quantity * i.UnitPrice),
                UserId = cart.UserId,
                TotalItems = cart.Items.Sum(i => i.Quantity)
            };
        }
    }
}
