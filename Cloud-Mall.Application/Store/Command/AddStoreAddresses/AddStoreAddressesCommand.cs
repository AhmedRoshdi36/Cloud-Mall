using Cloud_Mall.Application.DTOs.Store;
using MediatR;

namespace Cloud_Mall.Application.Store.Command.AddStoreAddresses
{
    public class AddStoreAddressesCommand : IRequest<ApiResponse<bool>>
    {
        public int StoreId { get; set; }
        public List<StoreAddressDTO> Addresses { get; set; }
    }
}
