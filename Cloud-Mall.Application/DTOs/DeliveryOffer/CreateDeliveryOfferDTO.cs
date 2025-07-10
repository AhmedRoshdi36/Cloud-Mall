namespace Cloud_Mall.Application.DTOs.DeliveryOffer
{
    public class CreateDeliveryOfferDTO
    {
        public int CustomerOrderID { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDays { get; set; }
    }
} 