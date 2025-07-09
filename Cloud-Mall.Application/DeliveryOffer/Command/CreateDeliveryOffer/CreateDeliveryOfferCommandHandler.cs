using Cloud_Mall.Application.DTOs.DeliveryOffer;
using Cloud_Mall.Application.DTOs.DeliveryCompany;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.DeliveryOffer.Command.CreateDeliveryOffer
{
    public class CreateDeliveryOfferCommandHandler : IRequestHandler<CreateDeliveryOfferCommand, DeliveryOfferDTO>
    {
        private readonly IDeliveryOfferRepository _deliveryOfferRepository;
        private readonly IDeliveryCompanyRepository _deliveryCompanyRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateDeliveryOfferCommandHandler(
            IDeliveryOfferRepository deliveryOfferRepository,
            IDeliveryCompanyRepository deliveryCompanyRepository,
            IOrderRepository orderRepository,
            ICurrentUserService currentUserService)
        {
            _deliveryOfferRepository = deliveryOfferRepository;
            _deliveryCompanyRepository = deliveryCompanyRepository;
            _orderRepository = orderRepository;
            _currentUserService = currentUserService;
        }

        public async Task<DeliveryOfferDTO> Handle(CreateDeliveryOfferCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            // Get delivery company for current user
            var deliveryCompany = await _deliveryCompanyRepository.GetByUserIdAsync(currentUserId);
            if (deliveryCompany == null)
            {
                throw new InvalidOperationException("Delivery company not found for current user.");
            }

            // Verify order exists
            var order = await _orderRepository.GetByIdAsync(request.CustomerOrderID);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            // Check if delivery company already made an offer for this order
            var existingOffers = await _deliveryOfferRepository.GetByCustomerOrderIdAsync(request.CustomerOrderID);
            if (existingOffers.Any(o => o.DeliveryCompanyID == deliveryCompany.ID))
            {
                throw new InvalidOperationException("You have already made an offer for this order.");
            }

            // Create delivery offer
            var deliveryOffer = new Domain.Entities.DeliveryOffer
            {
                Price = request.Price,
                EstimatedDays = request.EstimatedDays,
                OfferDate = DateTime.UtcNow,
                Status = Cloud_Mall.Domain.Enums.DeliveryOfferStatus.Pending,
                DeliveryCompanyID = deliveryCompany.ID,
                CustomerOrderID = request.CustomerOrderID
            };

            var createdOffer = await _deliveryOfferRepository.CreateAsync(deliveryOffer);

            return new DeliveryOfferDTO
            {
                ID = createdOffer.ID,
                Price = createdOffer.Price,
                EstimatedDays = createdOffer.EstimatedDays,
                OfferDate = createdOffer.OfferDate,
                Status = createdOffer.Status,
                DeliveryCompany = new DeliveryCompanyDTO
                {
                    ID = deliveryCompany.ID,
                    Name = deliveryCompany.Name,
                    CommercialSerialNumber = deliveryCompany.CommercialSerialNumber,
                    Phone = deliveryCompany.Phone,
                    Email = deliveryCompany.Email,
                    CreatedAt = deliveryCompany.CreatedAt,
                    IsActive = deliveryCompany.IsActive
                }
            };
        }
    }
} 