using MediatR;
using MyShop.WebApi.Domain.Dto;

namespace MyShop.WebApi.Applications.Queries.CartQueries
{
    public class GetCartQuery : IRequest<CartDto>
    {
        public string UserId { get; set; }
    }
}
