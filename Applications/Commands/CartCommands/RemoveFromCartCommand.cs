using MediatR;

namespace MyShop.WebApi.Applications.Commands.CartCommands
{
    public class RemoveFromCartCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public int CartItemId { get; set; }
    }
}