using MediatR;
using MyShop.WebApi.Domain.Dto;

namespace MyShop.WebApi.Applications.Queries.OrderQueries
{
    public class GetOrderByIdQuery : IRequest<OrderDto>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }
}
