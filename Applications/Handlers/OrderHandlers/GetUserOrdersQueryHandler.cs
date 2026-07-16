using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Queries.OrderQueries;
using MyShop.WebApi.Domain.Dto;
using MyShop.WebApi.Infrastructure.Data;

namespace MyShop.WebApi.Applications.Handlers.OrderHandlers
{
    public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, List<OrderDto>>
    {
        private readonly AppDbContext _context;

        public GetUserOrdersQueryHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<OrderDto>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Include(x => x.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == request.UserId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);

            var result = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User.UserName,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                Items = order.Items.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.ProductName,
                    ProductImage = item.Product.ImageUrl,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();

            return result;
        }
    }
}
