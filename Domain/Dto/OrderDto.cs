namespace MyShop.WebApi.Domain.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
