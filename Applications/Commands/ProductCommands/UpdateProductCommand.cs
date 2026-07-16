using MediatR;
using MyShop.WebApi.Domain.Enums;

namespace MyShop.WebApi.Applications.Commands.ProductCommands
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public ProductCategory? Category { get; set; }
        public string? ImageUrl { get; set; }
    }
}
