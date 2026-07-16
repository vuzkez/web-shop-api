using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Commands.CartCommands;
using MyShop.WebApi.Infrastructure.Data;
using MyShop.WebApi.Applications.Common.Exceptions;
using MyShop.WebApi.Domain.Entities;

namespace MyShop.WebApi.Applications.Handlers.CartHandlers
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, bool>
    {
        private readonly AppDbContext _context;

        public RemoveFromCartCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.UserId == request.UserId,cancellationToken);
            if (cart == null)
                throw new NotFoundException(nameof(Cart),request.UserId);

            var item = cart.Items.FirstOrDefault(x => x.Id == request.CartItemId);
            if (item == null)
                throw new NotFoundException(nameof(CartItem), request.UserId);

            cart.Items.Remove(item);
            cart.SetTotalPrice();
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
