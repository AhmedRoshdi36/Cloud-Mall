using Cloud_Mall.Domain.Enums;

namespace Cloud_Mall.Domain.Entities
{
    public class DeliveryOffer
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDays { get; set; }
        public DateTime OfferDate { get; set; }
        public DeliveryOfferStatus Status { get; set; } = DeliveryOfferStatus.Pending;
        
        public int DeliveryCompanyID { get; set; }
        public virtual DeliveryCompany DeliveryCompany { get; set; }
        
        public int CustomerOrderID { get; set; }
        public virtual CustomerOrder CustomerOrder { get; set; }
    }
} 