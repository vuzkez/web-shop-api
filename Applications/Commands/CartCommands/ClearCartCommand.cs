using MediatR;

namespace MyShop.WebApi.Applications.Commands.CartCommands
{
    public class ClearCartCommand : IRequest<bool>
    {
        public string UserId { get; set; }
    }
}