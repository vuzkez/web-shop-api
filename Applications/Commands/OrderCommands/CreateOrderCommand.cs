using MediatR;

namespace MyShop.WebApi.Applications.Commands.OrderCommands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public string UserId { get; set; }
    }
}
