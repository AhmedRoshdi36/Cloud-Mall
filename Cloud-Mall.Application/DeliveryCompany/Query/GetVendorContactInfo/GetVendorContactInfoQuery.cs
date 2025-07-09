using Cloud_Mall.Application.DTOs.DeliveryOffer;
using MediatR;

namespace Cloud_Mall.Application.DeliveryCompany.Query.GetVendorContactInfo
{
    public class GetVendorContactInfoQuery : IRequest<IEnumerable<VendorContactInfoDTO>>
    {
        public int CustomerOrderID { get; set; }
    }
} 