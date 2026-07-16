using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.WebApi.Applications.Commands.ProductCommands;
using MyShop.WebApi.Applications.Queries.ProductQueries;
using MyShop.WebApi.Domain.Dto;
using MyShop.WebApi.Domain.Enums;

namespace MyShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public ProductsController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _mediatR.Send(new GetProductsQuery(), new CancellationToken());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _mediatR.Send(new GetProductByIdQuery { Id = id });
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            var productId = await _mediatR.Send(command, new CancellationToken());
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand { Id = id };
            var isDeleted = await _mediatR.Send(command, new CancellationToken());
            return isDeleted ? NoContent() : NotFound();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var command = new UpdateProductCommand 
            { 
                Id = id,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl
            };
            var isPatched = await _mediatR.Send(command, new CancellationToken());
            return isPatched ? NoContent() : NotFound();
        }
    }
}
