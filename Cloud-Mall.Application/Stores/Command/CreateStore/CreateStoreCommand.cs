using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cloud_Mall.Application.Stores.Command.CreateStore
{
    public class CreateStoreCommand : IRequest<ApiResponse<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StoreCategoryID { get; set; }
        public IFormFile LogoFile { get; set; }
    }

}
