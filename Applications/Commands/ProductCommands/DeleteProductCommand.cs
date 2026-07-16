using MediatR;

namespace MyShop.WebApi.Applications.Commands.ProductCommands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
