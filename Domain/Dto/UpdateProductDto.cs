using MyShop.WebApi.Domain.Enums;

namespace MyShop.WebApi.Domain.Dto
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public ProductCategory? Category { get; set; }
        public string? ImageUrl { get; set; }
    }
}
