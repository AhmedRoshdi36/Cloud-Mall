using Cloud_Mall.Application.DTOs.DeliveryOffer;
using MediatR;

namespace Cloud_Mall.Application.DeliveryOffer.Command.CreateDeliveryOffer
{
    public class CreateDeliveryOfferCommand : IRequest<DeliveryOfferDTO>
    {
        public int CustomerOrderID { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDays { get; set; }
    }
} 