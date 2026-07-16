using MediatR;
using MyShop.WebApi.Domain.Dto;

namespace MyShop.WebApi.Applications.Queries.ProductQueries
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {

    }
}
