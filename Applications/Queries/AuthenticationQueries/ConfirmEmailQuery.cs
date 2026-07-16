using MediatR;

namespace MyShop.WebApi.Applications.Queries.AuthenticationQueries
{
    public class ConfirmEmailQuery : IRequest<bool>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
