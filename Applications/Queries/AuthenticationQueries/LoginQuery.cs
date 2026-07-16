using MediatR;

namespace MyShop.WebApi.Applications.Queries.AuthenticationQueries
{
    public class LoginQuery : IRequest<string> 
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CaptchaToken { get; set; }
    }
}