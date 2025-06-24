using Cloud_Mall.Application.DTOs.Store;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetAllVendorStoresQuery
{
    public class GetAllVendorStoresQuery : IRequest<ApiResponse<List<GetOneStoreDTO>>>
    {
    }
}
