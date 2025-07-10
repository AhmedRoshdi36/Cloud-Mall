using Cloud_Mall.Application.DTOs.DeliveryCompany;
using MediatR;

namespace Cloud_Mall.Application.DeliveryCompany.Command.RegisterDeliveryCompany
{
    public class RegisterDeliveryCompanyCommand : IRequest<DeliveryCompanyDTO>
    {
        public string Name { get; set; }
        public string CommercialSerialNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
} 