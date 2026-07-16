using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.WebApi.Applications.Commands.CartCommands;
using MyShop.WebApi.Applications.Queries.CartQueries;
using MyShop.WebApi.Domain.Dto;

namespace MyShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? throw new UnauthorizedAccessException("Пользователь не авторизован");
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _mediator.Send(new ClearCartCommand {UserId = userId});
            return NoContent();
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddCart([FromBody] AddToCartCommand command)
        {
            command.UserId = GetUserId();
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var cart = await _mediator.Send(new GetCartQuery { UserId = userId });
            return Ok(cart);
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var userId = GetUserId();
            var command = new RemoveFromCartCommand { UserId = userId, CartItemId = cartItemId };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateQuantityDto dto)
        {
            var userId = GetUserId();
            var command = new UpdateCartItemCommand
            {
                UserId = userId,
                CartItemId = cartItemId,
                Quantity = dto.Quantity
            };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
