using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.WebApi.Applications.Commands.CartCommands;
using MyShop.WebApi.Applications.Common.Exceptions;
using MyShop.WebApi.Domain.Entities;
using MyShop.WebApi.Infrastructure.Data;

namespace MyShop.WebApi.Applications.Handlers.CartHandlers
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand,bool>
    {
        private readonly AppDbContext _context;

        public ClearCartCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(x => x.UserId == request.UserId,cancellationToken);
            if (cart == null)
                throw new NotFoundException(nameof(Cart), request.UserId);

            cart.SetTotalPrice();
            cart.Items.Clear();
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
