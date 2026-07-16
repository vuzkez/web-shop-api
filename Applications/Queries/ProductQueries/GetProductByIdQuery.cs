using MediatR;
using MyShop.WebApi.Domain.Dto;

namespace MyShop.WebApi.Applications.Queries.ProductQueries
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }
}
