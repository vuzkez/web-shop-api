using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Commands.OrderCommands;
using MyShop.WebApi.Applications.Common.Exceptions;
using MyShop.WebApi.Domain.Entities;
using MyShop.WebApi.Infrastructure.Data;

namespace MyShop.WebApi.Applications.Handlers.OrderHandlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(AppDbContext context, ILogger<CreateOrderCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

            if (cart == null || !cart.Items.Any())
                throw new CustomValidationException("Корзина пуста или не найдена.");

            var order = new Order
            {
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
            };

            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.UnitPrice
                };
                order.Items.Add(orderItem);
            }

            order.SetTotalPrice();

            _context.Orders.Add(order);

            _context.CartItems.RemoveRange(cart.Items);
            cart.TotalPrice = 0;

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Заказ с id:{orderId} создан, время создания:{order.CreateAt}, создал заказ пользователь с id:{userId}"
                ,order.Id,order.CreatedAt,request.UserId);

            return order.Id;
        }
    }
}