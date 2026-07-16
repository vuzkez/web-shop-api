using MediatR;
using MyShop.WebApi.Domain.Dto;

namespace MyShop.WebApi.Applications.Queries.OrderQueries
{
    public class GetUserOrdersQuery : IRequest<List<OrderDto>>
    {
        public string UserId { get; set; }
    }
}
