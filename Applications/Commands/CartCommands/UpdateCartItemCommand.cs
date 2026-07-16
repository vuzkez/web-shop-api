using MediatR;

namespace MyShop.WebApi.Applications.Commands.CartCommands
{
    public class UpdateCartItemCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}