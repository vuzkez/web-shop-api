using MediatR;

namespace MyShop.WebApi.Applications.Commands.AuthenticationCommands
{
    public class RegisterCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string CaptchaToken { get; set; }
        public string? Telegram { get; set; }
    }
}
