namespace MyShop.WebApi.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; } 

        public void SetTotalPrice()
        {
            TotalPrice = Items.Sum(i => i.Quantity * i.UnitPrice);
        }
    }
}
