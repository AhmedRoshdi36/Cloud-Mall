namespace Cloud_Mall.Domain.Entities
{
    public class CustomerOrder
    {
        public int ID { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime OrderDate { get; set; }

        public string ClientID { get; set; }
        public virtual ApplicationUser Client { get; set; }

        public virtual ICollection<VendorOrder> VendorOrders { get; set; } = new List<VendorOrder>();
        public virtual ICollection<DeliveryOffer> DeliveryOffers { get; set; } = new List<DeliveryOffer>();
    }
}