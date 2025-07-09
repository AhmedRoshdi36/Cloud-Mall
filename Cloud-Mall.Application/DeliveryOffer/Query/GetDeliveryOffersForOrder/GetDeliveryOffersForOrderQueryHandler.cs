using Cloud_Mall.Application.DTOs.DeliveryOffer;
using Cloud_Mall.Application.DTOs.DeliveryCompany;
using Cloud_Mall.Application.Interfaces;
using MediatR;

namespace Cloud_Mall.Application.DeliveryOffer.Query.GetDeliveryOffersForOrder
{
    public class GetDeliveryOffersForOrderQueryHandler : IRequestHandler<GetDeliveryOffersForOrderQuery, IEnumerable<DeliveryOfferDTO>>
    {
        private readonly IDeliveryOfferRepository _deliveryOfferRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetDeliveryOffersForOrderQueryHandler(
            IDeliveryOfferRepository deliveryOfferRepository,
            ICurrentUserService currentUserService)
        {
            _deliveryOfferRepository = deliveryOfferRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<DeliveryOfferDTO>> Handle(GetDeliveryOffersForOrderQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            var offers = await _deliveryOfferRepository.GetByCustomerOrderIdAsync(request.CustomerOrderID);
            
            // Verify the current user is the order owner
            var firstOffer = offers.FirstOrDefault();
            if (firstOffer != null && firstOffer.CustomerOrder.ClientID != currentUserId)
            {
                throw new UnauthorizedAccessException("You can only view offers for your own orders.");
            }

            return offers.Select(offer => new DeliveryOfferDTO
            {
                ID = offer.ID,
                Price = offer.Price,
                EstimatedDays = offer.EstimatedDays,
                OfferDate = offer.OfferDate,
                Status = offer.Status,
                DeliveryCompany = new DeliveryCompanyDTO
                {
                    ID = offer.DeliveryCompany.ID,
                    Name = offer.DeliveryCompany.Name,
                    CommercialSerialNumber = offer.DeliveryCompany.CommercialSerialNumber,
                    Phone = offer.DeliveryCompany.Phone,
                    Email = offer.DeliveryCompany.Email,
                    CreatedAt = offer.DeliveryCompany.CreatedAt,
                    IsActive = offer.DeliveryCompany.IsActive
                }
            });
        }
    }
} 