using MyShop.WebApi.Domain.Enums;

namespace MyShop.WebApi.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string? ImageUrl { get; set; }
    }
}
