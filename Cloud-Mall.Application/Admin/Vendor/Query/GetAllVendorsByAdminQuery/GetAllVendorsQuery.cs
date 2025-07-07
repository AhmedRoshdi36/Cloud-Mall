using Cloud_Mall.Application.DTOs.Store;
using Cloud_Mall.Application.DTOs.Vendor;
using Cloud_Mall.Domain.Entities;
using MediatR;

namespace Cloud_Mall.Application.Admin.Vendor.Query.GetAllVendorsQuery;
public class GetAllVendorsByAdminQuery : IRequest<ApiResponse<List<VendorDTO>>>
{

}
