namespace Cloud_Mall.Domain.Entities
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTimeOfPurchase { get; set; }

        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
