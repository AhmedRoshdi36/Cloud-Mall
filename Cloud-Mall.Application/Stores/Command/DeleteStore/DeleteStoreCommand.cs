using MediatR;

namespace Cloud_Mall.Application.Stores.Command.DeleteStore
{
    public class DeleteStoreCommand : IRequest<ApiResponse<bool>>
    {
        public int storeId { get; set; }
    }
}
