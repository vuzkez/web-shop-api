namespace MyShop.WebApi.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public List<CartItem> Items { get; set; } = new();
        public decimal TotalPrice {  get; set; }

        public void SetTotalPrice()
        {
            TotalPrice = Items.Sum(i => i.Quantity * i.UnitPrice);
        }
    }
}
