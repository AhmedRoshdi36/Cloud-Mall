using Cloud_Mall.Application.DTOs.DeliveryOffer;
using MediatR;

namespace Cloud_Mall.Application.DeliveryOffer.Query.GetDeliveryOffersForOrder
{
    public class GetDeliveryOffersForOrderQuery : IRequest<IEnumerable<DeliveryOfferDTO>>
    {
        public int CustomerOrderID { get; set; }
    }
} 