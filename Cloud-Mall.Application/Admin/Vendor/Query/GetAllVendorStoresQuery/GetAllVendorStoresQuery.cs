using Cloud_Mall.Application.DTOs.Store;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetAllVendorStoresQuery;

public class GetAllVendorStoresByAdminQuery(string vendorId) : IRequest<ApiResponse<List<GetOneStoreDTO>>>
{
    public string VendorId { get; set; } = vendorId;
}
