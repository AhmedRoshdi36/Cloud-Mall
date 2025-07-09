using Cloud_Mall.Application.DTOs.DeliveryOffer;
using Cloud_Mall.Application.DTOs.DeliveryCompany;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Enums;
using MediatR;

namespace Cloud_Mall.Application.DeliveryOffer.Command.AcceptDeliveryOffer
{
    public class AcceptDeliveryOfferCommandHandler : IRequestHandler<AcceptDeliveryOfferCommand, DeliveryOfferDTO>
    {
        private readonly IDeliveryOfferRepository _deliveryOfferRepository;
        private readonly ICurrentUserService _currentUserService;

        public AcceptDeliveryOfferCommandHandler(
            IDeliveryOfferRepository deliveryOfferRepository,
            ICurrentUserService currentUserService)
        {
            _deliveryOfferRepository = deliveryOfferRepository;
            _currentUserService = currentUserService;
        }

        public async Task<DeliveryOfferDTO> Handle(AcceptDeliveryOfferCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            // Get the offer to accept
            var offerToAccept = await _deliveryOfferRepository.GetByIdAsync(request.OfferID);
            if (offerToAccept == null)
            {
                throw new InvalidOperationException("Delivery offer not found.");
            }

            // Verify the current user is the order owner
            if (offerToAccept.CustomerOrder.ClientID != currentUserId)
            {
                throw new UnauthorizedAccessException("You can only accept offers for your own orders.");
            }

            // Check if offer is still pending
            if (offerToAccept.Status != DeliveryOfferStatus.Pending)
            {
                throw new InvalidOperationException("This offer is no longer available for acceptance.");
            }

            // Get all pending offers for this order
            var pendingOffers = await _deliveryOfferRepository.GetPendingOffersByCustomerOrderIdAsync(offerToAccept.CustomerOrderID);
            
            // Accept the selected offer
            offerToAccept.Status = DeliveryOfferStatus.Accepted;
            await _deliveryOfferRepository.UpdateAsync(offerToAccept);

            // Reject all other pending offers
            var offersToReject = pendingOffers.Where(o => o.ID != request.OfferID).ToList();
            foreach (var offer in offersToReject)
            {
                offer.Status = DeliveryOfferStatus.Rejected;
            }
            await _deliveryOfferRepository.UpdateMultipleAsync(offersToReject);

            return new DeliveryOfferDTO
            {
                ID = offerToAccept.ID,
                Price = offerToAccept.Price,
                EstimatedDays = offerToAccept.EstimatedDays,
                OfferDate = offerToAccept.OfferDate,
                Status = offerToAccept.Status,
                DeliveryCompany = new DeliveryCompanyDTO
                {
                    ID = offerToAccept.DeliveryCompany.ID,
                    Name = offerToAccept.DeliveryCompany.Name,
                    CommercialSerialNumber = offerToAccept.DeliveryCompany.CommercialSerialNumber,
                    Phone = offerToAccept.DeliveryCompany.Phone,
                    Email = offerToAccept.DeliveryCompany.Email,
                    CreatedAt = offerToAccept.DeliveryCompany.CreatedAt,
                    IsActive = offerToAccept.DeliveryCompany.IsActive
                }
            };
        }
    }
} 