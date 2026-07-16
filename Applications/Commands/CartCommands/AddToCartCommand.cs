using MediatR;

namespace MyShop.WebApi.Applications.Commands.CartCommands
{
    public class AddToCartCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
