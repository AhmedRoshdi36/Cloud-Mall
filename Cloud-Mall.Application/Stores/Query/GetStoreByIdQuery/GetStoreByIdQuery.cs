using Cloud_Mall.Application.DTOs.Store;
using MediatR;

namespace Cloud_Mall.Application.Stores.Query.GetStoreByIdQuery
{
    public class GetStoreByIdQuery : IRequest<GetOneStoreDTO>
    {
        public int StoreId { get; set; }
        public GetStoreByIdQuery(int storeId)
        {
            StoreId = storeId;
        }
    }
} 