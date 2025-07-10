using Cloud_Mall.Application.DTOs.DeliveryOffer;
using MediatR;

namespace Cloud_Mall.Application.DeliveryOffer.Command.AcceptDeliveryOffer
{
    public class AcceptDeliveryOfferCommand : IRequest<DeliveryOfferDTO>
    {
        public int OfferID { get; set; }
    }
} 