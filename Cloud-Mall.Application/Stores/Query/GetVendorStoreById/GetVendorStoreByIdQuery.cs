using Cloud_Mall.Application.DTOs.Store;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetVendorStoreById
{
    public class GetVendorStoreByIdQuery : IRequest<ApiResponse<StoreDTO>>
    {
        public int StoreId { get; set; }
    }
}
