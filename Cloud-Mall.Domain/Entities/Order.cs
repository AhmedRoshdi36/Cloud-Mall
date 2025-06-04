namespace Cloud_Mall.Domain.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingStreetAddress { get; set; }

        public string ClientID { get; set; }
        public virtual ApplicationUser Client { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
