using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyShop.WebApi.Applications.Commands.AuthenticationCommands;
using MyShop.WebApi.Applications.Queries.AuthenticationQueries;

namespace MyShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public AuthController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "Регистрация успешна. Проверьте почту для подтверждения." });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            await _mediator.Send(new ConfirmEmailQuery { UserId = userId, Token = token });
            return Ok(new { message = "Email успешно подтверждён!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery query)
        {
            var token = await _mediator.Send(query);
            return Ok(new { token });
        }
    }
}
