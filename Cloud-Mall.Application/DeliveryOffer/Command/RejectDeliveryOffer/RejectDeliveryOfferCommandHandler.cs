using Cloud_Mall.Application.DTOs.DeliveryOffer;
using Cloud_Mall.Application.DTOs.DeliveryCompany;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Enums;
using MediatR;

namespace Cloud_Mall.Application.DeliveryOffer.Command.RejectDeliveryOffer
{
    public class RejectDeliveryOfferCommandHandler : IRequestHandler<RejectDeliveryOfferCommand, DeliveryOfferDTO>
    {
        private readonly IDeliveryOfferRepository _deliveryOfferRepository;
        private readonly ICurrentUserService _currentUserService;

        public RejectDeliveryOfferCommandHandler(
            IDeliveryOfferRepository deliveryOfferRepository,
            ICurrentUserService currentUserService)
        {
            _deliveryOfferRepository = deliveryOfferRepository;
            _currentUserService = currentUserService;
        }

        public async Task<DeliveryOfferDTO> Handle(RejectDeliveryOfferCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            // Get the offer to reject
            var offerToReject = await _deliveryOfferRepository.GetByIdAsync(request.OfferID);
            if (offerToReject == null)
            {
                throw new InvalidOperationException("Delivery offer not found.");
            }

            // Verify the current user is the order owner
            if (offerToReject.CustomerOrder.ClientID != currentUserId)
            {
                throw new UnauthorizedAccessException("You can only reject offers for your own orders.");
            }

            // Check if offer is still pending
            if (offerToReject.Status != DeliveryOfferStatus.Pending)
            {
                throw new InvalidOperationException("This offer is no longer available for rejection.");
            }

            // Reject the offer
            offerToReject.Status = DeliveryOfferStatus.Rejected;
            await _deliveryOfferRepository.UpdateAsync(offerToReject);

            return new DeliveryOfferDTO
            {
                ID = offerToReject.ID,
                Price = offerToReject.Price,
                EstimatedDays = offerToReject.EstimatedDays,
                OfferDate = offerToReject.OfferDate,
                Status = offerToReject.Status,
                DeliveryCompany = new DeliveryCompanyDTO
                {
                    ID = offerToReject.DeliveryCompany.ID,
                    Name = offerToReject.DeliveryCompany.Name,
                    CommercialSerialNumber = offerToReject.DeliveryCompany.CommercialSerialNumber,
                    Phone = offerToReject.DeliveryCompany.Phone,
                    Email = offerToReject.DeliveryCompany.Email,
                    CreatedAt = offerToReject.DeliveryCompany.CreatedAt,
                    IsActive = offerToReject.DeliveryCompany.IsActive
                }
            };
        }
    }
} 