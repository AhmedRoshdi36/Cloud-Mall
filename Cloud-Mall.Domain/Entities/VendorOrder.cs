using Cloud_Mall.Domain.Enums;

namespace Cloud_Mall.Domain.Entities
{
    public class VendorOrder
    {
        public int ID { get; set; }
        public VendorOrderStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingStreetAddress { get; set; }

        public int CustomerOrderID { get; set; }
        public virtual CustomerOrder CustomerOrder { get; set; }

        public int StoreID { get; set; }
        public virtual Store Store { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}