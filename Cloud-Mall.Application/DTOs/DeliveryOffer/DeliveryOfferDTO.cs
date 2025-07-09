using Cloud_Mall.Domain.Enums;
using Cloud_Mall.Application.DTOs.DeliveryCompany;

namespace Cloud_Mall.Application.DTOs.DeliveryOffer
{
    public class DeliveryOfferDTO
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDays { get; set; }
        public DateTime OfferDate { get; set; }
        public DeliveryOfferStatus Status { get; set; }
        public DeliveryCompanyDTO DeliveryCompany { get; set; }
    }
} 