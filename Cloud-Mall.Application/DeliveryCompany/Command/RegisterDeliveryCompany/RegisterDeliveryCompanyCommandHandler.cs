using Cloud_Mall.Application.DTOs.DeliveryCompany;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cloud_Mall.Application.DeliveryCompany.Command.RegisterDeliveryCompany
{
    public class RegisterDeliveryCompanyCommandHandler : IRequestHandler<RegisterDeliveryCompanyCommand, DeliveryCompanyDTO>
    {
        private readonly IDeliveryCompanyRepository _deliveryCompanyRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterDeliveryCompanyCommandHandler(
            IDeliveryCompanyRepository deliveryCompanyRepository,
            UserManager<ApplicationUser> userManager)
        {
            _deliveryCompanyRepository = deliveryCompanyRepository;
            _userManager = userManager;
        }

        public async Task<DeliveryCompanyDTO> Handle(RegisterDeliveryCompanyCommand request, CancellationToken cancellationToken)
        {
            // Check if commercial serial number already exists
            if (await _deliveryCompanyRepository.CommercialSerialExistsAsync(request.CommercialSerialNumber))
            {
                throw new InvalidOperationException("Commercial serial number already exists.");
            }

            // Create user account
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.Phone,
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            var userResult = await _userManager.CreateAsync(user, request.Password);
            if (!userResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");
            }

            // Add delivery company role
            await _userManager.AddToRoleAsync(user, "Delivery");

            // Create delivery company
            var deliveryCompany = new Cloud_Mall.Domain.Entities.DeliveryCompany
            {
                Name = request.Name,
                CommercialSerialNumber = request.CommercialSerialNumber,
                Phone = request.Phone,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                UserID = user.Id
            };

            var createdDeliveryCompany = await _deliveryCompanyRepository.CreateAsync(deliveryCompany);

            return new DeliveryCompanyDTO
            {
                ID = createdDeliveryCompany.ID,
                Name = createdDeliveryCompany.Name,
                CommercialSerialNumber = createdDeliveryCompany.CommercialSerialNumber,
                Phone = createdDeliveryCompany.Phone,
                Email = createdDeliveryCompany.Email,
                CreatedAt = createdDeliveryCompany.CreatedAt,
                IsActive = createdDeliveryCompany.IsActive
            };
        }
    }
} 