namespace Cloud_Mall.Domain.Entities
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTimeOfPurchase { get; set; }

        public int VendorOrderID { get; set; }
        public virtual VendorOrder VendorOrder { get; set; }

        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}