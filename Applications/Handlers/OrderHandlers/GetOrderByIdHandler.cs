using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Common.Exceptions;
using MyShop.WebApi.Applications.Queries.OrderQueries;
using MyShop.WebApi.Domain.Dto;
using MyShop.WebApi.Domain.Entities;
using MyShop.WebApi.Infrastructure.Data;

namespace MyShop.WebApi.Applications.Handlers.OrderHandlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly AppDbContext _context;

        public GetOrderByIdHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null)
                throw new NotFoundException(nameof(Order), request.Id);

            if (order.UserId != request.UserId)
                throw new CustomValidationException("У вас нет доступа к этому заказу.");

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User.UserName,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.ProductName,
                    ProductImage = i.Product.ImageUrl,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
        }
    }
}
