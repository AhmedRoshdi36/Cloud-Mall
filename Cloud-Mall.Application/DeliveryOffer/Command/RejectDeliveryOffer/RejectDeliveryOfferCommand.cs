using Cloud_Mall.Application.DTOs.DeliveryOffer;
using MediatR;

namespace Cloud_Mall.Application.DeliveryOffer.Command.RejectDeliveryOffer
{
    public class RejectDeliveryOfferCommand : IRequest<DeliveryOfferDTO>
    {
        public int OfferID { get; set; }
    }
} 