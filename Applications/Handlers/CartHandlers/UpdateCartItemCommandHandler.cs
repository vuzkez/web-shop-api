using MediatR;
using MyShop.WebApi.Applications.Commands.CartCommands;
using MyShop.WebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Common.Exceptions;
using MyShop.WebApi.Domain.Entities;

namespace MyShop.WebApi.Applications.Handlers.CartHandlers
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateCartItemCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);
            if (cart == null)
                throw new NotFoundException(nameof(Cart), request.UserId);

            var cartItem = cart.Items.FirstOrDefault(i => i.Id == request.CartItemId);
            if (cartItem == null)
                throw new NotFoundException(nameof(CartItem), request.CartItemId);

            if (request.Quantity <= 0)
                _context.CartItems.Remove(cartItem);
            else
                cartItem.Quantity = request.Quantity;

            cart.SetTotalPrice();
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
